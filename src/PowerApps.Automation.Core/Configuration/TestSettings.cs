namespace PowerApps.Automation.Core.Configuration;

public sealed record TestSettings
{
    public string TestEnvironment { get; init; } = "QA";
    public string Browser { get; init; } = "chromium";
    public string? BrowserChannel { get; init; }
    public bool Headless { get; init; } = true;
    public float SlowMoMs { get; init; }
    public float TimeoutMs { get; init; } = 30_000;
    public float NavigationTimeoutMs { get; init; } = 60_000;
    public bool TraceEnabled { get; init; } = true;
    public bool VideoEnabled { get; init; }
    public string ArtifactsPath { get; init; } = "artifacts";

    public string? CanvasAppUrl { get; init; }
    public string? ModelDrivenAppUrl { get; init; }
    public string? ModelDrivenReadySelector { get; init; }
    public string StorageStatePath { get; init; } = ".playwright-auth/state.json";

    public string? DataverseBaseUrl { get; init; }
    public string? DataverseAccessToken { get; init; }

    public static TestSettings Load()
    {
        EnvFile.LoadIfPresent();

        return new TestSettings
        {
            TestEnvironment = Get("TEST_ENV", "QA"),
            Browser = Get("BROWSER", "chromium"),
            BrowserChannel = NullIfWhiteSpace(Environment.GetEnvironmentVariable("BROWSER_CHANNEL")),
            Headless = GetBool("HEADLESS", true),
            SlowMoMs = GetFloat("SLOW_MO_MS", 0),
            TimeoutMs = GetFloat("TIMEOUT_MS", 30_000),
            NavigationTimeoutMs = GetFloat("NAVIGATION_TIMEOUT_MS", 60_000),
            TraceEnabled = GetBool("TRACE_ENABLED", true),
            VideoEnabled = GetBool("VIDEO_ENABLED", false),
            ArtifactsPath = Get("ARTIFACTS_PATH", "artifacts"),
            CanvasAppUrl = NullIfWhiteSpace(Environment.GetEnvironmentVariable("CANVAS_APP_URL")),
            ModelDrivenAppUrl = NullIfWhiteSpace(Environment.GetEnvironmentVariable("MODEL_DRIVEN_APP_URL")),
            ModelDrivenReadySelector = NullIfWhiteSpace(Environment.GetEnvironmentVariable("MODEL_DRIVEN_READY_SELECTOR")),
            StorageStatePath = Get("AUTH_STORAGE_STATE_PATH", ".playwright-auth/state.json"),
            DataverseBaseUrl = NullIfWhiteSpace(Environment.GetEnvironmentVariable("DATAVERSE_BASE_URL")),
            DataverseAccessToken = NullIfWhiteSpace(Environment.GetEnvironmentVariable("DATAVERSE_ACCESS_TOKEN"))
        };
    }

    public string RequireCanvasAppUrl() =>
        CanvasAppUrl ?? throw new InvalidOperationException("CANVAS_APP_URL is required for Canvas App tests.");

    public string RequireModelDrivenAppUrl() =>
        ModelDrivenAppUrl ?? throw new InvalidOperationException("MODEL_DRIVEN_APP_URL is required for Model-Driven App tests.");

    public string RequireDataverseBaseUrl() =>
        DataverseBaseUrl ?? throw new InvalidOperationException("DATAVERSE_BASE_URL is required for Dataverse API tests.");

    public string RequireDataverseAccessToken() =>
        DataverseAccessToken ?? throw new InvalidOperationException("DATAVERSE_ACCESS_TOKEN is required for Dataverse API tests.");

    private static string Get(string key, string fallback) =>
        NullIfWhiteSpace(Environment.GetEnvironmentVariable(key)) ?? fallback;

    private static bool GetBool(string key, bool fallback) =>
        bool.TryParse(Environment.GetEnvironmentVariable(key), out var value) ? value : fallback;

    private static float GetFloat(string key, float fallback) =>
        float.TryParse(Environment.GetEnvironmentVariable(key), out var value) ? value : fallback;

    private static string? NullIfWhiteSpace(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
