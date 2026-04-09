# 🎮 Dave's Arcade - Game Catalog API

[![Build Status](https://github.com/DaveHatz23/DavesArcade/workflows/Build%20and%20Test/badge.svg)](https://github.com/DaveHatz23/DavesArcade/actions)
[![codecov](https://codecov.io/gh/DaveHatz23/DavesArcade/branch/main/graph/badge.svg?token=YOUR_BADGE_TOKEN)](https://codecov.io/gh/DaveHatz23/DavesArcade)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

A modern RESTful API for managing a game catalog, showcasing **Clean Architecture**, **Result Pattern**, and **Test-Driven Development** best practices.

> **🎯 Showcase Project**: This is a portfolio project demonstrating enterprise-level .NET development practices, not a production application.

---

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [Running Tests](#-running-tests)
- [Logging & Monitoring](#-logging--monitoring)
- [Caching](#-caching)
- [Project Structure](#-project-structure)
- [Design Decisions](#-design-decisions)
- [Roadmap](#-roadmap)

---

## ✨ Features

- ✅ **Clean Architecture** - Clear separation of concerns across Domain, Application, Infrastructure, and API layers
- ✅ **Vertical Slice Architecture** - Feature-focused endpoint organization
- ✅ **Result Pattern** - Type-safe error handling without exceptions
- ✅ **Global Exception Handling** - Centralized error management middleware
- ✅ **Repository Pattern** - Abstracted data access with in-memory implementation
- ✅ **Comprehensive Testing** - 75 tests with 83% coverage (Unit + Integration)
- ✅ **Structured Logging** - Serilog with console and file sinks for observability
- ✅ **OpenAPI/Swagger** - Interactive API documentation with per-version docs
- ✅ **API Versioning** - URL segment versioning (v1/v2) with Swagger integration
- ✅ **In-Memory Caching** - Cache-aside pattern with `IMemoryCache` for read performance
- ✅ **Minimal APIs** - Modern .NET 8 endpoint routing
- ✅ **Docker Support** - Containerized deployment with Docker and Docker Compose
- ✅ **Health Checks** - Built-in health monitoring endpoint
- ✅ **CI/CD Pipeline** - Automated builds and testing with GitHub Actions
- ✅ **Code Coverage** - Integrated coverage reporting with Codecov

---

## 🛠️ Tech Stack

| Layer | Technologies |
|-------|-------------|
| **Framework** | .NET 8.0 |
| **API** | ASP.NET Core Minimal APIs |
| **Versioning** | Asp.Versioning 8.x (URL segment) |
| **Caching** | `Microsoft.Extensions.Caching.Memory` (IMemoryCache) |
| **Testing** | xUnit, FluentAssertions, Coverlet |
| **Documentation** | Swagger/OpenAPI (per-version docs) |
| **Architecture** | Clean Architecture, Vertical Slices |
| **Patterns** | Repository, Result, Cache-Aside, CQRS-lite |
| **DevOps** | Docker, Docker Compose, GitHub Actions |
| **Logging** | Serilog (Console, File, Structured) |
| **Monitoring** | Health Checks, Code Coverage (Codecov) |

---

## 📊 Logging & Monitoring

The application uses **Serilog** for structured, production-ready logging with multiple output sinks.

### Log Outputs

**Console:**
```
[15:30:45 INF] Starting Dave's Arcade API
[15:30:50 INF] HTTP GET /games responded 200 in 45.2341 ms
[15:30:52 INF] Fetching game with a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d
[15:30:52 INF] Successfully retrieved game "Halo Infinite"
```

**Rolling Files:** `logs/davesarcade-YYYYMMDD.log`
```
2026-04-06 15:30:45.123 +00:00 [INF] Starting Dave's Arcade API {"MachineName":"SERVER-01","ThreadId":1}
2026-04-06 15:30:50.234 +00:00 [INF] HTTP GET /games responded 200 in 45.2341 ms {"RequestHost":"localhost:5000","UserAgent":"Mozilla/5.0..."}
```

#### Features

- ✅ **Structured Data**: Logs include typed properties for easy querying
- ✅ **Request Logging**: Automatic HTTP request/response logging
- ✅ **Performance Tracking**: Response times tracked for all endpoints
- ✅ **Enrichment**: Machine name, thread ID, and request context
- ✅ **Rolling Files**: Daily log rotation prevents disk space issues
- ✅ **Environment-Specific**: Different log levels per environment

#### Configuration

Logging is configured in `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

**Development environment** (`appsettings.Development.json`) logs at `Debug` level for detailed diagnostics.

#### View Logs

```bash
# Follow live logs
tail -f logs/davesarcade-20260406.log   # Linux/Mac
Get-Content logs/davesarcade-20260406.log -Wait  # Windows PowerShell

# Search logs
grep "ERROR" logs/*.log                 # Linux/Mac
Select-String -Pattern "ERROR" -Path logs/*.log  # Windows
```

---

## ⚡ Caching

The application uses the **Cache-Aside pattern** with `IMemoryCache` (built into .NET — no external dependencies required).

### How It Works

```
Read:   Request → Cache hit?  ──Yes──► Return cached value
                      │
                      No
                      │
                      ▼
               Query data source → Store in cache → Return value

Write:  Create / Update / Delete → Invalidate affected cache entries
```

### Cache Policy

| Operation | Cache Key | TTL | Invalidated By |
|-----------|-----------|-----|----------------|
| `GET /games` | `games:all` | 5 minutes | Create |
| `GET /games/{id}` | `games:{id}` | 10 minutes | Update or Delete of that ID |
| `POST /games` | — | — | Removes `games:all` |
| `PUT /games/{id}` | — | — | Removes `games:all` + `games:{id}` |
| `DELETE /games/{id}` | — | — | Removes `games:all` + `games:{id}` |

### Implementation

Cache keys are defined as constants in `CacheKeys.cs` to avoid magic strings:

```csharp
internal static class CacheKeys
{
    internal const string AllGames = "games:all";
    internal static string GameById(Guid id) => $"games:{id}";
}
```

Caching is registered entirely within the Infrastructure layer — no changes required in `Program.cs`:

```csharp
// DependencyInjection.cs
services.AddMemoryCache();
services.AddSingleton<IGameRepository, InMemoryGameRepository>();
```

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with **Vertical Slice** organization:

![Clean Architecture](https://raw.githubusercontent.com/DaveHatz23/DavesArcade/main/docs/architecture.png)

- **Domain Layer**: Contains enterprise-wide business rules. Doesn't depend on any other layers. (Entities, Value Objects, Domain Events)
- **Application Layer**: Holds application-specific business rules. Features are organized in a vertical slice manner. (Commands, Queries, Handlers, DTOs)
- **Infrastructure Layer**: Contains implementation for external integrations such as databases, file systems, and web services. (EF Core, Repositories, Services)
- **API Layer**: Thin layer for exposing the application functionality over HTTP. Implements minimal API endpoints and ties everything together.

### Key Architectural Decisions

- **Result Pattern**: All repository methods return `Result<T>` for explicit success/failure handling
- **Extension Methods**: `ToHttpResult()` centralizes Result → HTTP response mapping
- **In-Memory Repository**: Simplifies development and testing without database dependencies
- **Vertical Slices**: Each feature (GetGames, CreateGame, etc.) is self-contained
- **URL Segment Versioning**: Routes are prefixed with `/v{version}` (e.g. `/v1/games`, `/v2/games`)
- **Cache-Aside Pattern**: Repository checks the cache before hitting the data source; writes invalidate affected entries

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop/) for containerized deployment
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation & Running

#### Option 1: Run with .NET CLI

1. **Clone the repository**
   ```bash
   git clone https://github.com/DaveHatz23/DavesArcade.git
   cd DavesArcade
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the API**
   ```bash
   cd src/DavesArcade.Api
   dotnet run
   ```

4. **Access the API**
   - API: `https://localhost:7xxx` (check console for actual port)
   - Swagger UI v1: `https://localhost:7xxx/swagger` → select **v1**
   - Swagger UI v2: `https://localhost:7xxx/swagger` → select **v2**

#### Option 2: Run with Docker 🐳 (Recommended)

**Windows (PowerShell):**
```powershell
git clone https://github.com/DaveHatz23/DavesArcade.git
cd DavesArcade/infra
.\start.ps1
```

**Linux/Mac:**
```bash
git clone https://github.com/DaveHatz23/DavesArcade.git
cd DavesArcade/infra
chmod +x start.sh
./start.sh
```

**Or manually:**
```bash
cd infra
docker-compose up -d
```

**Access the API:**
- API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`
- Health Check: `http://localhost:5000/health`

> 📖 **See [Infrastructure README](infra/README.md) and [Docker Documentation](docs/DOCKER.md) for detailed Docker usage.**

---

## 📡 API Endpoints

For complete API documentation with detailed examples, see **[API Documentation](docs/API.md)**.

### API Versioning

All endpoints are versioned via URL segment. The current versions are:

| Version | Base Path | Status |
|---------|-----------|--------|
| v1 | `/v1/games` | Stable |
| v2 | `/v2/games` | Enhanced (filtering + pagination) |

### Quick Reference

#### V1 Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/v1/games` | Get all games |
| `GET` | `/v1/games/{id}` | Get game by ID |
| `POST` | `/v1/games` | Create new game |
| `PUT` | `/v1/games/{id}` | Update existing game |
| `DELETE` | `/v1/games/{id}` | Delete game |

#### V2 Endpoints (Enhanced)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/v2/games?genre=RPG&pageNumber=1&pageSize=10` | Get games with filtering and pagination |
| `GET` | `/v2/games/{id}` | Get game by ID |
| `POST` | `/v2/games` | Create new game |
| `PUT` | `/v2/games/{id}` | Update existing game |
| `DELETE` | `/v2/games/{id}` | Delete game |

### Example: Get Game by ID

**Request**
````````http
GET /v1/games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d HTTP/1.1
Host: localhost:5000
Accept: application/json
````````

**Response (200 OK)**
````````json
{
  "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
  "name": "Halo Infinite",
  "genre": "Shooter",
  "price": 59.99,
  "releaseDate": "2021-12-08",
  "imageUri": "/images/halo-infinite.jpg",
  "lastUpdatedBy": "admin"
}
````````

### Example: Get Games with Filtering and Pagination (V2)

**Request**
````````http
GET /v2/games?genre=RPG&pageNumber=1&pageSize=5 HTTP/1.1
Host: localhost:5000
Accept: application/json
````````

**Response (200 OK)**
````````json
{
  "data": [
    {
      "id": "b2c3d4e5-f6a7-4b5c-9d1e-2f3a4b5c6d7e",
      "name": "Elden Ring",
      "genre": "RPG",
      "price": 59.99,
      "releaseDate": "2022-02-25",
      "imageUri": "/images/elden-ring.jpg",
      "lastUpdatedBy": "admin"
    }
  ],
  "pagination": {
    "currentPage": 1,
    "pageSize": 5,
    "totalCount": 2,
    "totalPages": 1
  },
  "filters": {
    "genre": "RPG"
  }
}
````````

---

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test
```

**Test Summary:**
- 47 Unit Tests (Repository, Extensions, Result Pattern)
- 28 Integration Tests (Full HTTP pipeline, all endpoints)
- **75 Total Tests** with **83% Code Coverage** ✅

### Run Tests with Coverage

**Quick Method (Automated Script):**
```bash
# Windows
.\coverage.ps1

# Linux/Mac
./coverage.sh
```

**Manual Method:**
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory:"./coverage"

# Generate HTML report
reportgenerator -reports:"coverage/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# Open report
start coverage-report/index.html  # Windows
open coverage-report/index.html   # Mac
xdg-open coverage-report/index.html  # Linux
```

### Test Coverage by Project

- **DavesArcade.Domain**: 100% coverage
- **DavesArcade.Application**: 100% coverage
- **DavesArcade.Infrastructure**: 99.4% coverage
- **DavesArcade.Api**: 63.6% coverage (DTOs excluded)
- **Overall**: 83% code coverage

### Run Specific Test Types

```bash
# Run only unit tests
dotenu test --filter "FullyQualifiedName~Unit"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~Integration"

# Run tests for specific feature
dotnet test --filter "FullyQualifiedName~GameRepository"
```

---

## 📁 Project Structure

```
DavesArcade/
├── .github/
│   └── workflows/              # GitHub Actions CI/CD
├── docs/
│   ├── API.md                  # Complete API documentation
│   └── DOCKER.md               # Docker usage guide
├── infra/
│   ├── Dockerfile              # Multi-stage Docker build
│   ├── docker-compose.yml      # Container orchestration
│   ├── .dockerignore           # Docker build exclusions
│   ├── start.ps1/start.sh      # Quick start scripts
│   └── README.md               # Infrastructure docs
├── src/
│   ├── DavesArcade.Api/        # API Layer (Endpoints, Middleware)
│   │   ├── Endpoints/
│   │   │   └── Games/          # Vertical slice endpoints
│   │   ├── Extensions/
│   │   │   └── ResultExtensions.cs
│   │   └── Middleware/
│   │       └── GlobalExceptionHandler.cs
│   ├── DavesArcade.Application/  # Application Layer (DTOs, Interfaces)
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Results/
│   ├── DavesArcade.Domain/     # Domain Layer (Entities)
│   │   └── Entities/
│   └── DavesArcade.Infrastructure/  # Infrastructure Layer (Data Access, Caching)
│       ├── Caching/
│       │   └── CacheKeys.cs        # Cache key constants
│       └── Persistence/
│           └── InMemory/
│               └── InMemoryGameRepository.cs
├── tests/
│   └── DavesArcade.Tests/      # Unit & Integration tests
│       ├── Unit/               # Unit tests (47 tests)
│       │   ├── Api/Extensions/
│       │   └── Infrastructure/Repositories/
│       └── Integration/        # Integration tests (28 tests)
│           ├── CustomWebApplicationFactory.cs
│           ├── GamesEndpointsTests.cs
│           └── HealthCheckTests.cs
├── coverage.ps1                # Coverage report generator (Windows)
├── coverage.sh                 # Coverage report generator (Linux/Mac)
├── .gitignore
├── LICENSE
└── README.md
```

---

## 💡 Design Decisions

### Why Clean Architecture?

- **Testability**: Business logic is independent of frameworks and databases
- **Maintainability**: Clear boundaries between layers
- **Flexibility**: Easy to swap implementations (e.g., in-memory → SQL database)

### Why Result Pattern?

Instead of throwing exceptions for expected failures:

- Provides a consistent, type-safe way to handle success and failure
- Enables returning additional context or data on failure
- Mimics built-in types like `Nullable<T>` or `ValueTask<T>`

### Why Minimal APIs?

- Lightweight and fast to develop
- Reduces boilerplate code for simple CRUD operations
- Seamlessly integrates with ASP.NET Core's pipeline

### Why Vertical Slices?

- Each feature is self-contained, reducing dependencies between features
- Easier to understand and navigate codebase
- Aligns with the way APIs are consumed, making it intuitive

### Why In-Memory Repository?

For this showcase project:
- ✅ Zero setup required - just `dotnet run`
- ✅ Fast iteration during development
- ✅ Perfect for unit testing
- ✅ Demonstrates repository pattern abstraction
- ✅ Easy to swap for EF Core later

### Why Serilog for Logging?

- **Structured Logging**: Log data as typed properties, not just strings
- **Multiple Sinks**: Write to console, files, databases, or cloud services
- **Performance**: Minimal overhead with async logging
- **Flexibility**: Environment-specific configuration
- **Industry Standard**: Used in production by thousands of companies
- **Queryable**: Structured data enables easy log searching and analysis

### Why API Versioning?

- **Non-breaking changes**: New features (e.g. filtering, pagination) can be introduced in v2 without affecting existing v1 consumers
- **Explicit contracts**: Each version has its own Swagger doc, making the contract per version clear
- **URL segment strategy**: `/v1/games` is the most discoverable and cache-friendly versioning approach
- **Future-proof**: Adding v3 requires only a new endpoint mapping and version registration

### Why In-Memory Caching?

- **Zero infrastructure**: `IMemoryCache` is built into .NET — no Redis, no external services required
- **Cache-Aside pattern**: Explicit control over what gets cached and when entries are invalidated
- **Correct invalidation**: Writes (Create/Update/Delete) immediately remove stale entries — no stale reads
- **Constant cache keys**: `CacheKeys.cs` keeps all key strings in one place, preventing bugs from typos
- **Upgrade path**: Swapping to `IDistributedCache` (Redis) later only requires changing the Infrastructure registration, not any repository logic

---

## 🗺️ Roadmap

### Phase 1: Core API ✅ (Complete)
- [x] Clean Architecture setup
- [x] CRUD endpoints for games
- [x] Result pattern implementation
- [x] Global exception handling
- [x] Unit tests (90%+ coverage)
- [x] Swagger documentation

### Phase 2: Enhanced Features ✅ (Complete)
- [x] GitHub Actions CI/CD pipeline
- [x] Docker support
- [x] Health checks endpoint
- [x] Code coverage reporting with Codecov
- [x] Comprehensive API documentation
- [x] Infrastructure automation scripts
- [x] Integration tests (28 tests)
- [x] Structured logging with Serilog
- [x] API versioning (v1/v2 with URL segment strategy)
- [x] In-memory caching (Cache-Aside pattern with IMemoryCache)

### Phase 3: Advanced Features 📅 (Planned)
- [ ] FluentValidation for request validation
- [ ] PostgreSQL with EF Core
- [ ] Authentication/Authorization (JWT)
- [ ] Rate limiting
- [ ] Distributed caching (Redis)
- [ ] Performance benchmarking
- [ ] Blazor/Next.js frontend (optional)
