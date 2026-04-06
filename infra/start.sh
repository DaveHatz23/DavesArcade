#!/bin/bash
# Dave's Arcade - Docker Quick Start
# Linux/Mac bash script to start the application

echo "?? Dave's Arcade - Starting Docker containers..."
echo ""

# Change to script directory
cd "$(dirname "$0")"

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "? Docker is not running. Please start Docker."
    exit 1
fi
echo "? Docker is running"

# Start containers
echo ""
echo "Building and starting containers..."
docker-compose up --build -d

if [ $? -eq 0 ]; then
    echo ""
    echo "? Containers started successfully!"
    echo ""
    echo "?? API Endpoints:"
    echo "   • API:         http://localhost:5000"
    echo "   • Swagger UI:  http://localhost:5000/swagger"
    echo "   • Health:      http://localhost:5000/health"
    echo ""
    echo "?? Useful Commands:"
    echo "   • View logs:   docker-compose logs -f"
    echo "   • Stop:        docker-compose down"
    echo "   • Restart:     docker-compose restart"
    echo ""
else
    echo ""
    echo "? Failed to start containers. Check the error messages above."
    exit 1
fi
