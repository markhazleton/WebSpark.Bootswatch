#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Runs tests for each target framework separately
.DESCRIPTION
    This script runs tests for .NET 8.0, 9.0, and 10.0 individually, 
    providing separate test results for each framework.
.PARAMETER Framework
    Specific framework to test (net8.0, net9.0, net10.0). If not specified, tests all frameworks.
.PARAMETER Configuration
    Build configuration (Debug or Release). Default is Debug.
.EXAMPLE
    .\run-multi-framework-tests.ps1
    Runs tests for all frameworks
.EXAMPLE
    .\run-multi-framework-tests.ps1 -Framework net8.0
    Runs tests only for .NET 8.0
#>

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('net8.0', 'net9.0', 'net10.0')]
    [string]$Framework,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'

# Colors for output
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

function Test-Framework {
    param(
        [string]$TargetFramework,
        [string]$Config
    )
    
    Write-Header "Testing $TargetFramework"
    
    try {
        # Restore dependencies
        Write-Host "Restoring dependencies..." -ForegroundColor $infoColor
        dotnet restore
        
        # Build for specific framework
        Write-Host "Building for $TargetFramework..." -ForegroundColor $infoColor
        dotnet build --configuration $Config --framework $TargetFramework --no-restore
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Build failed for $TargetFramework" -ForegroundColor $errorColor
            return $false
        }
        
        # Run tests for specific framework
        Write-Host "Running tests for $TargetFramework..." -ForegroundColor $infoColor
        $testResultFile = "TestResults/test-results-$TargetFramework.trx"
        
        dotnet test `
            --configuration $Config `
            --framework $TargetFramework `
            --no-build `
            --verbosity normal `
            --logger "trx;LogFileName=$testResultFile" `
            --logger "console;verbosity=detailed"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "? Tests passed for $TargetFramework" -ForegroundColor $successColor
            return $true
        } else {
            Write-Host "? Tests failed for $TargetFramework" -ForegroundColor $errorColor
            return $false
        }
    }
    catch {
        Write-Host "? Error testing $TargetFramework : $_" -ForegroundColor $errorColor
        return $false
    }
}

# Main execution
Write-Header "Multi-Framework Test Runner"
Write-Host "Configuration: $Configuration" -ForegroundColor $infoColor

$frameworks = if ($Framework) { @($Framework) } else { @('net8.0', 'net9.0', 'net10.0') }
$results = @{}

foreach ($fw in $frameworks) {
    $results[$fw] = Test-Framework -TargetFramework $fw -Config $Configuration
}

# Summary
Write-Header "Test Results Summary"

$allPassed = $true
foreach ($fw in $frameworks) {
    $status = if ($results[$fw]) { "? PASSED" } else { "? FAILED"; $allPassed = $false }
    $color = if ($results[$fw]) { $successColor } else { $errorColor }
    Write-Host "$fw : $status" -ForegroundColor $color
}

Write-Host ""

if ($allPassed) {
    Write-Host "All tests passed!" -ForegroundColor $successColor
    exit 0
} else {
    Write-Host "Some tests failed!" -ForegroundColor $errorColor
    exit 1
}
