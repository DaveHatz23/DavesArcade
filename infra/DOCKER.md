# ?? Docker Setup for Dave's Arcade API

This document explains how to run Dave's Arcade API using Docker.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- Or [Docker Engine](https://docs.docker.com/engine/install/) (Linux)

## Quick Start

### Option 1: Using Docker Compose (Recommended)

```bash
# Build and run with a single command
docker-compose up --build

# Run in detached mode (background)
docker-compose up -d

# View logs
docker-compose logs -f

# Stop containers
docker-compose down
```

The API will be available at:
- **HTTP**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health

### Option 2: Using Docker CLI

```bash
# Build the image
docker build -t davesarcade-api:latest .

# Run the container
docker run -d \
  --name davesarcade-api \
  -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  davesarcade-api:latest

# View logs
docker logs -f davesarcade-api

# Stop and remove container
docker stop davesarcade-api
docker rm davesarcade-api
```

## Available Endpoints

Once running, access:

- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health
- **Get All Games**: http://localhost:5000/games
- **Get Game by ID**: http://localhost:5000/games/{id}

## Docker Commands Reference

### Container Management

```bash
# List running containers
docker ps

# List all containers (including stopped)
docker ps -a

# Stop a container
docker stop davesarcade-api

# Start a stopped container
docker start davesarcade-api

# Restart a container
docker restart davesarcade-api

# Remove a container
docker rm davesarcade-api

# View container logs
docker logs davesarcade-api

# Follow logs in real-time
docker logs -f davesarcade-api

# Execute commands inside container
docker exec -it davesarcade-api /bin/bash
```

### Image Management

```bash
# List images
docker images

# Remove an image
docker rmi davesarcade-api:latest

# Remove unused images
docker image prune

# Build with no cache
docker build --no-cache -t davesarcade-api:latest .
```

### Docker Compose Commands

```bash
# Build images
docker-compose build

# Start services
docker-compose up

# Start in detached mode
docker-compose up -d

# Stop services
docker-compose stop

# Stop and remove containers
docker-compose down

# View logs
docker-compose logs

# Follow logs
docker-compose logs -f

# Rebuild and restart
docker-compose up --build

# Scale services (if needed)
docker-compose up --scale davesarcade-api=3
```

## Environment Variables

Configure the application using environment variables:

| Variable | Default | Description |
|----------|---------|-------------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Application environment (Development/Production) |
| `ASPNETCORE_URLS` | `http://+:8080` | URLs to listen on |

Example with custom environment:

```bash
docker run -d \
  -p 5000:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:8080 \
  davesarcade-api:latest
```

## Health Checks

The container includes a health check endpoint at `/health`.

**Check container health:**

```bash
# View health status
docker inspect --format='{{.State.Health.Status}}' davesarcade-api

# Manual health check
curl http://localhost:5000/health
```

Health check runs every 30 seconds and marks container as unhealthy after 3 consecutive failures.

## Troubleshooting

### Container won't start

```bash
# Check logs
docker logs davesarcade-api

# Check if port is already in use
netstat -an | grep 5000  # Linux/Mac
netstat -an | findstr 5000  # Windows
```

### Can't access the API

1. Verify container is running:
   ```bash
   docker ps
   ```

2. Check health status:
   ```bash
   curl http://localhost:5000/health
   ```

3. Verify port mapping:
   ```bash
   docker port davesarcade-api
   ```

### Rebuild after code changes

```bash
# Stop and remove old container
docker-compose down

# Rebuild and start
docker-compose up --build
```

## Production Deployment

### Build for Production

```bash
docker build \
  --build-arg ASPNETCORE_ENVIRONMENT=Production \
  -t davesarcade-api:1.0.0 \
  .
```

### Tag for Registry

```bash
# Tag for Docker Hub
docker tag davesarcade-api:latest yourusername/davesarcade-api:1.0.0

# Push to Docker Hub
docker push yourusername/davesarcade-api:1.0.0

# Tag for GitHub Container Registry
docker tag davesarcade-api:latest ghcr.io/davehatz23/davesarcade-api:1.0.0

# Push to GitHub Container Registry
docker push ghcr.io/davehatz23/davesarcade-api:1.0.0
```

## Security Best Practices

? **Non-root user** - Container runs as non-root user `appuser`  
? **Minimal base image** - Uses `aspnet:8.0` runtime (not SDK)  
? **Health checks** - Container self-monitors health  
? **No secrets in image** - Uses environment variables for config  

## Multi-Architecture Support

Build for multiple platforms:

```bash
# Build for AMD64 and ARM64
docker buildx build \
  --platform linux/amd64,linux/arm64 \
  -t davesarcade-api:latest \
  .
```

## Docker Hub / GHCR

### Publish to Docker Hub

1. Login:
   ```bash
   docker login
   ```

2. Tag and push:
   ```bash
   docker tag davesarcade-api:latest davehatz23/davesarcade-api:latest
   docker push davehatz23/davesarcade-api:latest
   ```

### Publish to GitHub Container Registry

1. Create Personal Access Token with `write:packages` permission

2. Login:
   ```bash
   echo $GITHUB_TOKEN | docker login ghcr.io -u davehatz23 --password-stdin
   ```

3. Tag and push:
   ```bash
   docker tag davesarcade-api:latest ghcr.io/davehatz23/davesarcade:latest
   docker push ghcr.io/davehatz23/davesarcade:latest
   ```

## Performance Tips

- Use multi-stage builds (already implemented)
- Leverage Docker layer caching
- Use `.dockerignore` to exclude unnecessary files
- Set appropriate memory limits in production

## Support

For issues or questions:
- GitHub Issues: https://github.com/DaveHatz23/DavesArcade/issues
- Documentation: See main [README.md](../README.md)

---

**Quick Reference Card:**

```bash
# Start everything
docker-compose up -d

# View logs
docker-compose logs -f

# Stop everything
docker-compose down

# Rebuild after changes
docker-compose up --build
```
