# Dave's Arcade - Docker Quick Start
# Windows PowerShell script to start the application

Write-Host "?? Dave's Arcade - Starting Docker containers..." -ForegroundColor Cyan
Write-Host ""

# Change to script directory
Set-Location $PSScriptRoot

# Check if Docker is running
try {
    docker info | Out-Null
    Write-Host "? Docker is running" -ForegroundColor Green
} catch {
    Write-Host "? Docker is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

# Start containers
Write-Host ""
Write-Host "Building and starting containers..." -ForegroundColor Yellow
docker-compose up --build -d

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "? Containers started successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? API Endpoints:" -ForegroundColor Cyan
    Write-Host "   • API:         http://localhost:5000" -ForegroundColor White
    Write-Host "   • Swagger UI:  http://localhost:5000/swagger" -ForegroundColor White
    Write-Host "   • Health:      http://localhost:5000/health" -ForegroundColor White
    Write-Host ""
    Write-Host "?? Useful Commands:" -ForegroundColor Cyan
    Write-Host "   • View logs:   docker-compose logs -f" -ForegroundColor White
    Write-Host "   • Stop:        docker-compose down" -ForegroundColor White
    Write-Host "   • Restart:     docker-compose restart" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "? Failed to start containers. Check the error messages above." -ForegroundColor Red
    exit 1
}
