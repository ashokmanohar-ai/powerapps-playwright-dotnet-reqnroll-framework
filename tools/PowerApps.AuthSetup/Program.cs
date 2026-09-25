using Microsoft.Playwright;
using PowerApps.Automation.Core.Configuration;

var settings = TestSettings.Load();
var targetUrl = settings.CanvasAppUrl ?? settings.ModelDrivenAppUrl;

if (string.IsNullOrWhiteSpace(targetUrl))
{
    Console.Error.WriteLine("Set CANVAS_APP_URL or MODEL_DRIVEN_APP_URL before running authentication setup.");
    return 2;
}

var outputPath = settings.StorageStatePath;
var outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
if (!string.IsNullOrWhiteSpace(outputDirectory))
{
    Directory.CreateDirectory(outputDirectory);
}

using var playwright = await Playwright.CreateAsync();
var launchOptions = new BrowserTypeLaunchOptions
{
    Headless = false
};

if (!string.IsNullOrWhiteSpace(settings.BrowserChannel))
{
    launchOptions.Channel = settings.BrowserChannel;
}

var browser = await playwright.Chromium.LaunchAsync(launchOptions);
var context = await browser.NewContextAsync(new BrowserNewContextOptions
{
    ViewportSize = new ViewportSize { Width = 1440, Height = 900 }
});
var page = await context.NewPageAsync();

Console.WriteLine($"Opening: {targetUrl}");
await page.GotoAsync(targetUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

Console.WriteLine();
Console.WriteLine("Complete Microsoft Entra sign-in in the browser, including MFA if required.");
Console.WriteLine("Wait until the Power App is fully loaded, then return here and press Enter.");
Console.ReadLine();

await context.StorageStateAsync(new BrowserContextStorageStateOptions { Path = outputPath });
Console.WriteLine($"Authentication state saved to: {Path.GetFullPath(outputPath)}");
Console.WriteLine("Treat this file as a secret. It is excluded from Git by the framework .gitignore.");

await context.CloseAsync();
await browser.CloseAsync();
return 0;
