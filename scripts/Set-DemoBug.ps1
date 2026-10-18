param([Parameter(Mandatory)][ValidateSet('Introduce','Fix')][string]$Mode)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$file = Join-Path $projectRoot 'src/DiscountDemo/PriceCalculator.cs'
$source = [IO.File]::ReadAllText($file)
if ($Mode -eq 'Introduce') {
    if (!$source.Contains('percent is < 0 or > 100')) { throw 'Expected correct guard was not found.' }
    $source = $source.Replace('percent is < 0 or > 100', 'percent > 100')
} else {
    if (!$source.Contains('if (percent > 100)')) { throw 'Expected demo bug was not found.' }
    $source = $source.Replace('if (percent > 100)', 'if (percent is < 0 or > 100)')
    $versionFile = Join-Path $projectRoot 'src/DiscountDemo/build-info.json'
    $version = Get-Content $versionFile -Raw | ConvertFrom-Json
    $version.version = '1.1.0'
    $version | ConvertTo-Json | Set-Content $versionFile -Encoding utf8
}
[IO.File]::WriteAllText($file, $source, [Text.UTF8Encoding]::new($false))
Write-Host "Demo mode: $Mode. Review git diff before committing."
