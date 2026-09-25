using Microsoft.Playwright;

namespace PowerApps.Automation.Core.PowerApps;

public sealed class ModelDrivenAppPage
{
    private readonly IPage _page;
    private readonly float _timeoutMs;

    public ModelDrivenAppPage(IPage page, float timeoutMs)
    {
        _page = page;
        _timeoutMs = timeoutMs;
    }

    public async Task NavigateAsync(string appUrl, string? readySelector = null)
    {
        await _page.GotoAsync(appUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
        await WaitForReadyAsync(readySelector);
    }

    public async Task WaitForReadyAsync(string? readySelector = null)
    {
        var selector = string.IsNullOrWhiteSpace(readySelector) ? "body" : readySelector;
        await _page.Locator(selector).First.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = _timeoutMs
        });
    }

    public async Task ClickCommandAsync(string commandName)
    {
        await _page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = commandName, Exact = true })
            .First.ClickAsync();
    }

    public async Task FillFieldByLabelAsync(string label, string value)
    {
        await _page.GetByLabel(label, new PageGetByLabelOptions { Exact = true })
            .First.FillAsync(value);
    }

    public async Task OpenRecordByTextAsync(string recordText)
    {
        await _page.GetByText(recordText, new PageGetByTextOptions { Exact = true })
            .First.ClickAsync();
    }
}
