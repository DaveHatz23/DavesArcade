# Dave's Arcade - Docker Stop Script
# Windows PowerShell script to stop and remove containers

Write-Host "?? Dave's Arcade - Stopping Docker containers..." -ForegroundColor Cyan
Write-Host ""

# Change to script directory
Set-Location $PSScriptRoot

# Stop and remove containers
docker-compose down

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "? Containers stopped and removed successfully!" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "? Failed to stop containers. Check the error messages above." -ForegroundColor Red
    exit 1
}
