#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Publishes WebSpark.Bootswatch NuGet package to NuGet.org
.DESCRIPTION
    This script publishes the WebSpark.Bootswatch package and symbols to NuGet.org.
    Requires a NuGet API key to be provided or stored in environment variable.
.PARAMETER ApiKey
    NuGet API key for authentication. If not provided, will look for NUGET_API_KEY environment variable.
.PARAMETER Version
    Package version to publish. Default is 1.31.0
.PARAMETER Source
    NuGet source URL. Default is https://api.nuget.org/v3/index.json
.PARAMETER PackageDirectory
    Directory containing the .nupkg files. Default is ./nupkgs
.PARAMETER SkipDuplicate
    If specified, will not fail if package version already exists
.PARAMETER WhatIf
    If specified, shows what would be published without actually publishing
.EXAMPLE
    .\publish-nuget-package.ps1 -ApiKey "your-api-key-here"
    Publishes version 1.31.0 to NuGet.org
.EXAMPLE
    .\publish-nuget-package.ps1 -ApiKey "your-api-key" -WhatIf
    Shows what would be published without actually publishing
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$ApiKey = $env:NUGET_API_KEY,
    
    [Parameter(Mandatory=$false)]
    [string]$Version = "1.31.0",
    
    [Parameter(Mandatory=$false)]
    [string]$Source = "https://api.nuget.org/v3/index.json",
    
    [Parameter(Mandatory=$false)]
    [string]$PackageDirectory = ".\nupkgs",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipDuplicate,
    
    [Parameter(Mandatory=$false)]
    [switch]$WhatIf
)

$ErrorActionPreference = 'Stop'

# Colors
$successColor = 'Green'
$errorColor = 'Red'
$infoColor = 'Cyan'
$warningColor = 'Yellow'

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "???????????????????????????????????????????????????????" -ForegroundColor $infoColor
    Write-Host " $Message" -ForegroundColor $infoColor
    Write-Host "???????????????????????????????????????????????????????" -ForegroundColor $infoColor
    Write-Host ""
}

Write-Header "WebSpark.Bootswatch NuGet Package Publisher"

# Validate API Key
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    Write-Host "? ERROR: NuGet API Key is required!" -ForegroundColor $errorColor
    Write-Host ""
    Write-Host "Please provide API key using one of these methods:" -ForegroundColor $warningColor
    Write-Host "  1. Parameter: -ApiKey 'your-api-key'" -ForegroundColor $warningColor
    Write-Host "  2. Environment variable: `$env:NUGET_API_KEY = 'your-api-key'" -ForegroundColor $warningColor
    Write-Host ""
    Write-Host "To get your API key:" -ForegroundColor $infoColor
    Write-Host "  1. Go to https://www.nuget.org/account/apikeys" -ForegroundColor $infoColor
    Write-Host "  2. Create or use an existing API key" -ForegroundColor $infoColor
    Write-Host "  3. Ensure the key has 'Push' permission" -ForegroundColor $infoColor
    exit 1
}

# Define package files
$packageFile = Join-Path $PackageDirectory "WebSpark.Bootswatch.$Version.nupkg"
$symbolsFile = Join-Path $PackageDirectory "WebSpark.Bootswatch.$Version.snupkg"

# Verify package files exist
if (-not (Test-Path $packageFile)) {
    Write-Host "? ERROR: Package file not found: $packageFile" -ForegroundColor $errorColor
    Write-Host "Please build the package first using:" -ForegroundColor $warningColor
    Write-Host "  dotnet pack -c Release" -ForegroundColor $warningColor
    exit 1
}

Write-Host "?? Package Information:" -ForegroundColor $infoColor
Write-Host "  Version: $Version" -ForegroundColor White
Write-Host "  Package: $packageFile" -ForegroundColor White
Write-Host "  Size: $((Get-Item $packageFile).Length / 1MB) MB" -ForegroundColor White

if (Test-Path $symbolsFile) {
    Write-Host "  Symbols: $symbolsFile" -ForegroundColor White
    Write-Host "  Symbols Size: $((Get-Item $symbolsFile).Length / 1KB) KB" -ForegroundColor White
} else {
    Write-Host "  Symbols: Not found (optional)" -ForegroundColor $warningColor
}

Write-Host ""

if ($WhatIf) {
    Write-Host "?? WHAT-IF MODE: The following would be published:" -ForegroundColor $warningColor
    Write-Host "  Package: $packageFile" -ForegroundColor White
    Write-Host "  To: $Source" -ForegroundColor White
    if (Test-Path $symbolsFile) {
        Write-Host "  Symbols: $symbolsFile" -ForegroundColor White
    }
    Write-Host ""
    Write-Host "Run without -WhatIf to actually publish" -ForegroundColor $infoColor
    exit 0
}

# Confirm before publishing
Write-Host "??  You are about to publish to NuGet.org!" -ForegroundColor $warningColor
Write-Host "   This action cannot be undone." -ForegroundColor $warningColor
Write-Host ""
$confirmation = Read-Host "Type 'yes' to continue, or anything else to cancel"

if ($confirmation -ne 'yes') {
    Write-Host "? Publication cancelled by user" -ForegroundColor $warningColor
    exit 0
}

Write-Host ""
Write-Header "Publishing Package"

try {
    # Build the command arguments
    $pushArgs = @(
        'nuget', 'push'
        $packageFile
        '--api-key', $ApiKey
        '--source', $Source
    )
    
    if ($SkipDuplicate) {
        $pushArgs += '--skip-duplicate'
    }
    
    # Publish the package
    Write-Host "?? Publishing package to NuGet.org..." -ForegroundColor $infoColor
    Write-Host "   Command: dotnet nuget push (with API key hidden)" -ForegroundColor Gray
    
    & dotnet @pushArgs
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Package published successfully!" -ForegroundColor $successColor
        
        # Publish symbols if they exist
        if (Test-Path $symbolsFile) {
            Write-Host ""
            Write-Host "?? Publishing symbols package..." -ForegroundColor $infoColor
            
            $symbolsPushArgs = @(
                'nuget', 'push'
                $symbolsFile
                '--api-key', $ApiKey
                '--source', $Source
            )
            
            if ($SkipDuplicate) {
                $symbolsPushArgs += '--skip-duplicate'
            }
            
            & dotnet @symbolsPushArgs
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "? Symbols published successfully!" -ForegroundColor $successColor
            } else {
                Write-Host "??  Symbols publish failed (non-critical)" -ForegroundColor $warningColor
            }
        }
        
        Write-Host ""
        Write-Header "Publication Complete"
        Write-Host "? Package WebSpark.Bootswatch $Version has been published!" -ForegroundColor $successColor
        Write-Host ""
        Write-Host "?? Next Steps:" -ForegroundColor $infoColor
        Write-Host "  1. Wait 5-10 minutes for NuGet.org to index the package" -ForegroundColor White
        Write-Host "  2. Verify at: https://www.nuget.org/packages/WebSpark.Bootswatch/$Version" -ForegroundColor White
        Write-Host "  3. Test installation: dotnet add package WebSpark.Bootswatch --version $Version" -ForegroundColor White
        Write-Host "  4. Update demo application to use new version" -ForegroundColor White
        Write-Host "  5. Create GitHub release with changelog" -ForegroundColor White
        Write-Host ""
        
    } else {
        Write-Host "? Package publish failed!" -ForegroundColor $errorColor
        exit 1
    }
    
} catch {
    Write-Host "? ERROR: $_" -ForegroundColor $errorColor
    exit 1
}
