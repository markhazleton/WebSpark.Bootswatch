# Frontend build script for WebSpark.Bootswatch
Write-Host "===== Running WebSpark.Bootswatch Frontend Build =====" -ForegroundColor Green
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location -Path $scriptPath
Write-Host "Current directory: $PWD" -ForegroundColor Yellow

Write-Host "Running npm clean..." -ForegroundColor Cyan
npm run clean

Write-Host "Running npm build..." -ForegroundColor Cyan
npm run build

Write-Host "===== Frontend Build Complete =====" -ForegroundColor Green
