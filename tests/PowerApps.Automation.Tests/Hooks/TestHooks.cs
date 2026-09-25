using PowerApps.Automation.Core.Browser;
using PowerApps.Automation.Core.Configuration;
using Reqnroll;

namespace PowerApps.Automation.Tests.Hooks;

[Binding]
public sealed class TestHooks
{
    private const string SettingsKey = "TestSettings";
    private const string SessionKey = "PlaywrightSession";
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario(Order = 0)]
    public async Task BeforeScenarioAsync()
    {
        var settings = TestSettings.Load();
        _scenarioContext[SettingsKey] = settings;

        var tags = _scenarioContext.ScenarioInfo.Tags;
        var requiresUi = tags.Contains("ui", StringComparer.OrdinalIgnoreCase);
        if (!requiresUi)
        {
            return;
        }

        var session = new PlaywrightSession(settings, _scenarioContext.ScenarioInfo.Title);
        await session.StartAsync();
        _scenarioContext[SessionKey] = session;
    }

    [AfterScenario(Order = 100)]
    public async Task AfterScenarioAsync()
    {
        if (!_scenarioContext.ContainsKey(SessionKey))
        {
            return;
        }

        var session = (PlaywrightSession)_scenarioContext[SessionKey];
        var failed = _scenarioContext.TestError is not null;

        try
        {
            await session.CompleteAsync(failed);
        }
        finally
        {
            await session.DisposeAsync();
        }
    }
}
