using NUnit.Framework;
using PowerApps.Automation.Core.PowerApps;
using Reqnroll;

namespace PowerApps.Automation.Tests.Steps;

[Binding]
public sealed class ModelDrivenSteps
{
    private readonly ScenarioContext _scenarioContext;
    private ModelDrivenAppPage? _modelDriven;

    public ModelDrivenSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("I open the configured Model-Driven App")]
    public async Task GivenIOpenTheConfiguredModelDrivenApp()
    {
        var settings = _scenarioContext.Settings();
        _modelDriven = new ModelDrivenAppPage(_scenarioContext.Session().Page, settings.TimeoutMs);
        await _modelDriven.NavigateAsync(settings.RequireModelDrivenAppUrl(), settings.ModelDrivenReadySelector);
    }

    [Then("the Model-Driven App URL should contain {string}")]
    public void ThenTheModelDrivenAppUrlShouldContain(string expected)
    {
        Assert.That(_scenarioContext.Session().Page.Url, Does.Contain(expected).IgnoreCase);
    }

    [When("I click Model-Driven command {string}")]
    public async Task WhenIClickModelDrivenCommand(string commandName)
    {
        await RequireModelDriven().ClickCommandAsync(commandName);
    }

    [When("I fill Model-Driven field {string} with {string}")]
    public async Task WhenIFillModelDrivenFieldWith(string label, string value)
    {
        await RequireModelDriven().FillFieldByLabelAsync(label, value);
    }

    private ModelDrivenAppPage RequireModelDriven() =>
        _modelDriven ?? throw new InvalidOperationException("Model-Driven App has not been opened. Run the Given step first.");
}
