# 🎮 Dave's Arcade - Game Catalog API

[![Build Status](https://github.com/DaveHatz23/DavesArcade/workflows/Build%20and%20Test/badge.svg)](https://github.com/DaveHatz23/DavesArcade/actions)
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

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with **Vertical Slice** organization:

![Clean Architecture](https://raw.githubusercontent.com/DaveHatz23/DavesArcade/main/docs/architecture.png)

- **Domain Layer**: Contains enterprise-wide business rules. Doesn’t depend on any other layers. (Entities, Value Objects, Domain Events)
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
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation & Running

1. **Clone the repository**
   ```bash
   git clone https://github.com/DaveHatz23/DavesArcade.git
   ```

2. **Navigate to the `src/Api` directory and run the project**:
   ```bash
   cd src/Api
   dotnet run
   ```

3. **Run the API**: The API will be available at `http://localhost:5000`.
4. **Access Swagger UI**: Open `http://localhost:5000/swagger` in your browser to interact with the API documentation.

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
````````

# Response
````````markdown
{
  "id": "a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d",
  "title": "Sample Game",
  "genre": "Action",
  "releaseDate": "2023-01-01",
  "developer": "XYZ Studios",
  "publisher": "ABC Games",
  "price": 29.99
}
```

---

## 🚧 Roadmap

Future enhancements and features planned for the project:
- [ ] Add more unit tests to cover 100% of the codebase
- [ ] Implement CI/CD pipeline using GitHub Actions
- [ ] Add support for Blazor frontend
- [ ] Dockerize the application for easier deployment
- [ ] Integrate with PostgreSQL and MongoDB as shown in the diagrams
- [ ] Enhance API documentation with more examples and tutorials

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

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/DaveHatz23/DavesArcade/blob/main/LICENSE) file for details.

---

## 💻 Acknowledgments

Inspiration and references from:
- [Clean Architecture](https://ardalis.com/clean-architecture/) by Steve Smith
- [Vertical Slice Architecture](https://michiduz.dev/posts/aspnet/clean-architecture-in-aspnet-core-3/) by Mikito Oka
- [Results in C#](https://jimcloud.wordpress.com/2020/07/24/c-results-zip-throw/) by Jimmy Bogard
- [Minimal APIs in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0) by Microsoft Docs

And special thanks to the .NET community for their amazing tools and support!

---

### Run with Code Coverage

````````markdown
### Test Coverage

- **InMemoryGameRepository**: 100% coverage (all CRUD operations)
- **ResultExtensions**: 100% coverage (HTTP result mapping)
- **Overall**: 90%+ code coverage

---

## 📁 Project Structure

````````

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

### Phase 2: Enhanced Features 🚧 (Planned)
- [ ] FluentValidation for request validation
- [ ] GitHub Actions CI/CD pipeline
- [ ] Docker support
- [ ] Health checks endpoint
- [ ] Structured logging (Serilog)
- [ ] API versioning

### Phase 3: Advanced Features 📅 (Future)
- [ ] PostgreSQL with EF Core
- [ ] Authentication/Authorization (JWT)
- [ ] Rate limiting
- [ ] Caching (Redis)
- [ ] Integration tests
- [ ] Blazor/Next.js frontend

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**David Hatzenbuehler**

- GitHub: [@DaveHatz23](https://github.com/DaveHatz23)
- LinkedIn: [Your LinkedIn](https://linkedin.com/in/davehatz) 

---

## 🙏 Acknowledgments

- Inspired by Clean Architecture principles by Robert C. Martin
- Result pattern inspired by functional programming concepts
- Built as a portfolio showcase project demonstrating enterprise .NET development

---

**⭐ If you find this project helpful, please consider giving it a star!**
