using Microsoft.Playwright;
using PowerApps.Automation.Core.Configuration;

namespace PowerApps.Automation.Core.Api;

public sealed class DataverseApiClient : IAsyncDisposable
{
    private readonly IPlaywright _playwright;
    private readonly IAPIRequestContext _request;

    private DataverseApiClient(IPlaywright playwright, IAPIRequestContext request)
    {
        _playwright = playwright;
        _request = request;
    }

    public static async Task<DataverseApiClient> CreateAsync(TestSettings settings)
    {
        var baseUrl = settings.RequireDataverseBaseUrl().TrimEnd('/');
        var token = settings.RequireDataverseAccessToken();
        var playwright = await Playwright.CreateAsync();

        var headers = new Dictionary<string, string>
        {
            ["Authorization"] = $"Bearer {token}",
            ["Accept"] = "application/json",
            ["OData-MaxVersion"] = "4.0",
            ["OData-Version"] = "4.0",
            ["Content-Type"] = "application/json"
        };

        var request = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = baseUrl,
            ExtraHTTPHeaders = headers
        });

        return new DataverseApiClient(playwright, request);
    }

    public async Task<(int Status, string Body)> WhoAmIAsync()
    {
        var response = await _request.GetAsync("/api/data/v9.2/WhoAmI");
        return (response.Status, await response.TextAsync());
    }

    public async Task<(int Status, string Body)> GetAsync(string relativeUrl)
    {
        var response = await _request.GetAsync(relativeUrl);
        return (response.Status, await response.TextAsync());
    }

    public async Task<(int Status, string Body)> PostAsync(string relativeUrl, object payload)
    {
        var response = await _request.PostAsync(relativeUrl, new APIRequestContextOptions
        {
            DataObject = payload
        });
        return (response.Status, await response.TextAsync());
    }

    public async Task<(int Status, string Body)> PatchAsync(string relativeUrl, object payload)
    {
        var response = await _request.PatchAsync(relativeUrl, new APIRequestContextOptions
        {
            DataObject = payload
        });
        return (response.Status, await response.TextAsync());
    }

    public async Task<int> DeleteAsync(string relativeUrl)
    {
        var response = await _request.DeleteAsync(relativeUrl);
        return response.Status;
    }

    public async ValueTask DisposeAsync()
    {
        await _request.DisposeAsync();
        _playwright.Dispose();
    }
}
