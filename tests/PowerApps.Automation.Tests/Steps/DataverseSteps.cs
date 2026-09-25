using NUnit.Framework;
using PowerApps.Automation.Core.Api;
using PowerApps.Automation.Core.Configuration;
using Reqnroll;

namespace PowerApps.Automation.Tests.Steps;

[Binding]
public sealed class DataverseSteps
{
    private readonly ScenarioContext _scenarioContext;
    private int _status;
    private string _body = string.Empty;

    public DataverseSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("Dataverse API is configured")]
    public void GivenDataverseApiIsConfigured()
    {
        var settings = _scenarioContext.Settings();
        Assert.That(settings.DataverseBaseUrl, Is.Not.Null.And.Not.Empty, "Set DATAVERSE_BASE_URL.");
        Assert.That(settings.DataverseAccessToken, Is.Not.Null.And.Not.Empty, "Set DATAVERSE_ACCESS_TOKEN.");
    }

    [When("I request WhoAmI from Dataverse")]
    public async Task WhenIRequestWhoAmIFromDataverse()
    {
        await using var client = await DataverseApiClient.CreateAsync(_scenarioContext.Settings());
        (_status, _body) = await client.WhoAmIAsync();
    }

    [Then("Dataverse should return HTTP {int}")]
    public void ThenDataverseShouldReturnHttp(int expectedStatus)
    {
        Assert.That(_status, Is.EqualTo(expectedStatus), _body);
    }

    [Then("the Dataverse response should contain {string}")]
    public void ThenTheDataverseResponseShouldContain(string expectedText)
    {
        Assert.That(_body, Does.Contain(expectedText));
    }
}
