$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot
Push-Location $Root
try {
    dotnet run --project tools/PowerApps.AuthSetup/PowerApps.AuthSetup.csproj
}
finally {
    Pop-Location
}
