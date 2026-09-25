using Microsoft.Playwright;
using PowerApps.Automation.Core.Configuration;
using PowerApps.Automation.Core.Utils;

namespace PowerApps.Automation.Core.Browser;

public sealed class PlaywrightSession : IAsyncDisposable
{
    private readonly TestSettings _settings;
    private readonly string _scenarioArtifactPath;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private bool _traceStarted;
    private bool _traceStopped;

    public IBrowserContext Context { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;

    public PlaywrightSession(TestSettings settings, string scenarioName)
    {
        _settings = settings;
        _scenarioArtifactPath = Path.Combine(settings.ArtifactsPath, FileNameSanitizer.Sanitize(scenarioName));
    }

    public async Task StartAsync()
    {
        Directory.CreateDirectory(_scenarioArtifactPath);
        _playwright = await Playwright.CreateAsync();

        var browserType = _settings.Browser.Trim().ToLowerInvariant() switch
        {
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium
        };

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless,
            SlowMo = _settings.SlowMoMs
        };

        if (!string.IsNullOrWhiteSpace(_settings.BrowserChannel))
        {
            launchOptions.Channel = _settings.BrowserChannel;
        }

        _browser = await browserType.LaunchAsync(launchOptions);

        var contextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = false,
            ViewportSize = new ViewportSize { Width = 1440, Height = 900 }
        };

        if (File.Exists(_settings.StorageStatePath))
        {
            contextOptions.StorageStatePath = _settings.StorageStatePath;
        }

        if (_settings.VideoEnabled)
        {
            var videoPath = Path.Combine(_scenarioArtifactPath, "video");
            Directory.CreateDirectory(videoPath);
            contextOptions.RecordVideoDir = videoPath;
            contextOptions.RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 };
        }

        Context = await _browser.NewContextAsync(contextOptions);
        Context.SetDefaultTimeout(_settings.TimeoutMs);
        Context.SetDefaultNavigationTimeout(_settings.NavigationTimeoutMs);

        if (_settings.TraceEnabled)
        {
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
            _traceStarted = true;
        }

        Page = await Context.NewPageAsync();
    }

    public async Task CompleteAsync(bool failed)
    {
        if (failed && Page is not null)
        {
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = Path.Combine(_scenarioArtifactPath, "failure.png"),
                FullPage = true
            });
        }

        if (_traceStarted && !_traceStopped)
        {
            if (failed)
            {
                await Context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = Path.Combine(_scenarioArtifactPath, "trace.zip")
                });
            }
            else
            {
                await Context.Tracing.StopAsync();
            }

            _traceStopped = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_traceStarted && !_traceStopped && Context is not null)
        {
            await Context.Tracing.StopAsync();
            _traceStopped = true;
        }

        if (Context is not null)
        {
            await Context.CloseAsync();
        }

        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}
