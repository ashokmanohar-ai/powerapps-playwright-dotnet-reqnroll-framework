using NUnit.Framework;
using PowerApps.Automation.Core.Configuration;
using Reqnroll;

namespace PowerApps.Automation.Tests.Steps;

[Binding]
public sealed class FrameworkSteps
{
    private TestSettings? _settings;

    [Given("the framework configuration is loaded")]
    public void GivenTheFrameworkConfigurationIsLoaded()
    {
        _settings = TestSettings.Load();
    }

    [Then("the browser should default to {string}")]
    public void ThenTheBrowserShouldDefaultTo(string expectedBrowser)
    {
        Assert.That(_settings, Is.Not.Null);
        Assert.That(_settings!.Browser, Is.EqualTo(expectedBrowser).IgnoreCase);
    }

    [Then("the default timeout should be greater than zero")]
    public void ThenTheDefaultTimeoutShouldBeGreaterThanZero()
    {
        Assert.That(_settings, Is.Not.Null);
        Assert.That(_settings!.TimeoutMs, Is.GreaterThan(0));
    }
}
