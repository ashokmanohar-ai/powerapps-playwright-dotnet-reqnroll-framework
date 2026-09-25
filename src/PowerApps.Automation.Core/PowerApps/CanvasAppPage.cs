using Microsoft.Playwright;

namespace PowerApps.Automation.Core.PowerApps;

public sealed class CanvasAppPage
{
    private const string PlayerFrameSelector = "iframe[name=\"fullscreen-app-host\"]";
    private readonly IPage _page;
    private readonly float _timeoutMs;

    public CanvasAppPage(IPage page, float timeoutMs)
    {
        _page = page;
        _timeoutMs = timeoutMs;
    }

    public IFrameLocator Frame => _page.FrameLocator(PlayerFrameSelector);

    public ILocator Control(string controlName)
    {
        var escaped = controlName.Replace("\\", "\\\\").Replace("\"", "\\\"");
        return Frame.Locator($"[data-control-name=\"{escaped}\"]");
    }

    public async Task NavigateAsync(string appUrl)
    {
        await _page.GotoAsync(appUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
        await WaitForReadyAsync();
    }

    public async Task WaitForReadyAsync()
    {
        await Frame.Locator("body").WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = _timeoutMs
        });
    }

    public async Task ClickControlAsync(string controlName)
    {
        var control = Control(controlName);
        await control.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = _timeoutMs });
        await control.ClickAsync();
    }

    public async Task FillControlAsync(string controlName, string value)
    {
        var control = Control(controlName);
        await control.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = _timeoutMs });

        var editable = control.Locator("input, textarea, [contenteditable='true']");
        if (await editable.CountAsync() > 0)
        {
            await editable.First.FillAsync(value);
            return;
        }

        await control.FillAsync(value);
    }

    public async Task<string> GetControlTextAsync(string controlName)
    {
        var control = Control(controlName);
        await control.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = _timeoutMs });

        var editable = control.Locator("input, textarea");
        if (await editable.CountAsync() > 0)
        {
            return await editable.First.InputValueAsync();
        }

        return await control.InnerTextAsync();
    }
}
