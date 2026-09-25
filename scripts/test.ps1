param(
    [ValidateSet("framework", "canvas", "mda", "dataverse", "ui", "all")]
    [string]$Scope = "framework"
)

$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot
$Project = "tests/PowerApps.Automation.Tests/PowerApps.Automation.Tests.csproj"

$Filter = switch ($Scope) {
    "framework" { "TestCategory=framework" }
    "canvas" { "TestCategory=canvas" }
    "mda" { "TestCategory=mda" }
    "dataverse" { "TestCategory=dataverse" }
    "ui" { "TestCategory=ui" }
    default { $null }
}

Push-Location $Root
try {
    if ($Filter) {
        dotnet test $Project --filter $Filter
    }
    else {
        dotnet test $Project
    }
}
finally {
    Pop-Location
}
