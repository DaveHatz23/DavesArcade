#!/bin/bash
# Dave's Arcade - Docker Stop Script
# Linux/Mac bash script to stop and remove containers

echo "?? Dave's Arcade - Stopping Docker containers..."
echo ""

# Change to script directory
cd "$(dirname "$0")"

# Stop and remove containers
docker-compose down

if [ $? -eq 0 ]; then
    echo ""
    echo "? Containers stopped and removed successfully!"
    echo ""
else
    echo ""
    echo "? Failed to stop containers. Check the error messages above."
    exit 1
fi
