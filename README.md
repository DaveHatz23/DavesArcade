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
- ✅ **Comprehensive Testing** - Unit tests with 90%+ coverage using xUnit and FluentAssertions
- ✅ **OpenAPI/Swagger** - Interactive API documentation
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
| **Testing** | xUnit, FluentAssertions, Coverlet |
| **Documentation** | Swagger/OpenAPI |
| **Architecture** | Clean Architecture, Vertical Slices |
| **Patterns** | Repository, Result, CQRS-lite |
| **DevOps** | Docker, Docker Compose, GitHub Actions |
| **Monitoring** | Health Checks, Code Coverage (Codecov) |

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
   - Swagger UI: `https://localhost:7xxx/swagger`

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

### Quick Reference

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/games` | Get all games |
| `GET` | `/games/{id}` | Get game by ID |
| `POST` | `/games` | Create new game |
| `PUT` | `/games/{id}` | Update existing game |
| `DELETE` | `/games/{id}` | Delete game |

### Example: Get Game by ID

**Request**
````````http
GET /games/a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d HTTP/1.1
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

---

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### View Coverage Report

```bash
# Install report generator (one-time)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"tests/DavesArcade.Tests/coverage.opencover.xml" -targetdir:"coverage-report"

# Open report
start coverage-report/index.html  # Windows
open coverage-report/index.html   # Mac
xdg-open coverage-report/index.html  # Linux
```

### Test Coverage

- **InMemoryGameRepository**: 100% coverage (all CRUD operations)
- **ResultExtensions**: 100% coverage (HTTP result mapping)
- **Overall**: 90%+ code coverage

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
│   └── DavesArcade.Infrastructure/  # Infrastructure Layer (Data Access)
│       └── Persistence/
│           └── InMemory/
├── tests/
│   └── DavesArcade.Tests/      # Unit tests
│       └── Unit/
│           ├── Api/Extensions/
│           └── Infrastructure/Repositories/
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

### Phase 3: Advanced Features 📅 (Planned)
- [ ] FluentValidation for request validation
- [ ] Structured logging (Serilog)
- [ ] API versioning
- [ ] PostgreSQL with EF Core
- [ ] Authentication/Authorization (JWT)
- [ ] Rate limiting
- [ ] Caching (Redis)
- [ ] Integration tests
- [ ] Blazor/Next.js frontend

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository

2. Create a new branch:
   ```bash
   git checkout -b feature/YourFeatureName
   ```

3. Make your changes and commit them:
   ```bash
   git commit -m "Add your message here"
   ```

4. Push to your forked repository:
   ```bash
   git push origin feature/YourFeatureName
   ```

5. Submit a pull request describing your changes and why they should be merged.

Please ensure your code adheres to the existing style and includes appropriate tests.

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**David Hatzenbuehler**

- GitHub: [@DaveHatz23](https://github.com/DaveHatz23)
- LinkedIn: [linkedin.com/in/davehatz](https://linkedin.com/in/davehatz)

---

## 🙏 Acknowledgments

- Inspired by Clean Architecture principles by Robert C. Martin
- Result pattern inspired by functional programming concepts
- Built as a portfolio showcase project demonstrating enterprise .NET development

---

**⭐ If you find this project helpful, please consider giving it a star!**
