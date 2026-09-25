using PowerApps.Automation.Core.Browser;
using PowerApps.Automation.Core.Configuration;
using Reqnroll;

namespace PowerApps.Automation.Tests.Steps;

internal static class ScenarioAccess
{
    public static TestSettings Settings(this ScenarioContext context) =>
        (TestSettings)context["TestSettings"];

    public static PlaywrightSession Session(this ScenarioContext context) =>
        (PlaywrightSession)context["PlaywrightSession"];
}
