param([int]$Port = 5080)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$workspaceSdk = Join-Path $projectRoot '../../work/tooling/dotnet/dotnet.exe'
if (Test-Path $workspaceSdk) {
    $dotnetCommand = (Resolve-Path $workspaceSdk).Path
} else {
    $dotnetCommand = (Get-Command dotnet -ErrorAction Stop).Source
}
Push-Location $projectRoot
try {
    & $dotnetCommand run --project src/DiscountDemo --urls "http://localhost:$Port"
} finally { Pop-Location }
