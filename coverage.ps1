# Dave's Arcade - Code Coverage Report Generator
# Generates HTML coverage report with integration tests

Write-Host "?? Dave's Arcade - Generating Code Coverage Report..." -ForegroundColor Cyan
Write-Host ""

# Clean previous coverage
Write-Host "Cleaning previous coverage data..." -ForegroundColor Yellow
Remove-Item -Path "tests/DavesArcade.Tests/TestResults" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "coverage-report" -Recurse -ErrorAction SilentlyContinue

# Run tests with coverage
Write-Host ""
Write-Host "Running all tests with coverage collection..." -ForegroundColor Yellow
dotnet test tests/DavesArcade.Tests/DavesArcade.Tests.csproj `
    --configuration Release `
    --collect:"XPlat Code Coverage" `
    --results-directory:"./coverage"

# Check if coverage file exists
$coverageFile = Get-ChildItem -Path "coverage" -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1

if ($null -eq $coverageFile) {
    Write-Host ""
    Write-Host "? Coverage file not found!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "? Coverage data collected: $($coverageFile.FullName)" -ForegroundColor Green

# Check if reportgenerator is installed
Write-Host ""
Write-Host "Checking for ReportGenerator..." -ForegroundColor Yellow

$reportGenInstalled = dotnet tool list -g | Select-String "dotnet-reportgenerator-globaltool"

if (-not $reportGenInstalled) {
    Write-Host "ReportGenerator not found. Installing..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
    Write-Host "? ReportGenerator installed" -ForegroundColor Green
} else {
    Write-Host "? ReportGenerator already installed" -ForegroundColor Green
}

# Generate HTML report
Write-Host ""
Write-Host "Generating HTML report..." -ForegroundColor Yellow

reportgenerator `
    -reports:"$($coverageFile.FullName)" `
    -targetdir:"coverage-report" `
    -reporttypes:"Html;TextSummary"

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "? Coverage report generated successfully!" -ForegroundColor Green
    Write-Host ""
    
    # Display summary
    Write-Host "?? Coverage Summary:" -ForegroundColor Cyan
    Get-Content "coverage-report/Summary.txt" | Write-Host
    
    Write-Host ""
    Write-Host "?? Report Location: coverage-report/index.html" -ForegroundColor Cyan
    Write-Host ""
    
    # Open report
    $openReport = Read-Host "Open coverage report in browser? (Y/n)"
    if ($openReport -eq "" -or $openReport -eq "Y" -or $openReport -eq "y") {
        Start-Process "coverage-report/index.html"
    }
    
    Write-Host ""
    Write-Host "? Done!" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "? Failed to generate report" -ForegroundColor Red
    exit 1
}
