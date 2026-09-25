$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot
Push-Location $Root
try {
    dotnet restore PowerApps.Automation.sln
    dotnet build PowerApps.Automation.sln

    $playwrightScript = Join-Path $Root "tests/PowerApps.Automation.Tests/bin/Debug/net10.0/playwright.ps1"
    if (-not (Test-Path $playwrightScript)) {
        throw "Playwright install script was not generated at $playwrightScript"
    }

    & pwsh $playwrightScript install chromium
    Write-Host "Framework setup complete." -ForegroundColor Green
}
finally {
    Pop-Location
}
