using NUnit.Framework;
using PowerApps.Automation.Core.PowerApps;
using Reqnroll;

namespace PowerApps.Automation.Tests.Steps;

[Binding]
public sealed class CanvasSteps
{
    private readonly ScenarioContext _scenarioContext;
    private CanvasAppPage? _canvas;

    public CanvasSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("I open the configured Canvas App")]
    public async Task GivenIOpenTheConfiguredCanvasApp()
    {
        var settings = _scenarioContext.Settings();
        _canvas = new CanvasAppPage(_scenarioContext.Session().Page, settings.TimeoutMs);
        await _canvas.NavigateAsync(settings.RequireCanvasAppUrl());
    }

    [When("I fill Canvas control {string} with {string}")]
    public async Task WhenIFillCanvasControlWith(string controlName, string value)
    {
        await RequireCanvas().FillControlAsync(controlName, value);
    }

    [When("I click Canvas control {string}")]
    public async Task WhenIClickCanvasControl(string controlName)
    {
        await RequireCanvas().ClickControlAsync(controlName);
    }

    [Then("Canvas control {string} should contain {string}")]
    public async Task ThenCanvasControlShouldContain(string controlName, string expectedText)
    {
        var actual = await RequireCanvas().GetControlTextAsync(controlName);
        Assert.That(actual, Does.Contain(expectedText));
    }

    private CanvasAppPage RequireCanvas() =>
        _canvas ?? throw new InvalidOperationException("Canvas App has not been opened. Run the Given step first.");
}
