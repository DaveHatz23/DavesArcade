Absolutely, Dave — here is the **full `copilot-guidance.md`** exactly as I originally generated it, cleanly formatted so you can drop it straight into any repo.

---

# **copilot-guidance.md**

## **Purpose**
These guidelines tell GitHub Copilot how to generate code, structure projects, and follow consistent engineering patterns across all repositories. Copilot should treat these rules as architectural constraints and coding standards.

---

## **1. Coding Style**
- Use **async/await** for all I/O‑bound operations.  
- Prefer **interfaces over concrete types**.  
- Avoid static classes unless they are pure utility helpers.  
- Use **dependency injection** for all services, clients, and repositories.  
- Follow **C# naming conventions** (PascalCase for types/methods, camelCase for locals/parameters).  
- Keep methods small, readable, and single‑purpose.  
- Avoid unnecessary abstractions — clarity > cleverness.

---

## **2. Architecture Rules**
- Follow **Clean Architecture** principles:
  - API layer → Application layer → Domain layer → Infrastructure layer  
  - No business logic in controllers or Azure Functions  
  - Domain layer has **no dependencies** on infrastructure  
- Use **MediatR** for commands/queries when appropriate.  
- Use **FluentValidation** for request validation.  
- Keep configuration in `appsettings.json` or environment variables — no hard‑coded values.  
- Prefer **composition over inheritance**.

---

## **3. API & Backend Conventions**
- Use **Minimal APIs** or **Controller‑based APIs** depending on project style, but keep endpoints thin.  
- Return standardized responses:
  - `200 OK` for success  
  - `400 BadRequest` for validation issues  
  - `404 NotFound` when applicable  
  - `500` only for unexpected errors  
- Use **DTOs** for all inbound/outbound models — never expose domain entities directly.  
- Add **OpenAPI/Swagger** annotations when helpful.

---

## **4. Azure & Cloud Patterns**
- For Azure Functions:
  - Keep functions small and orchestrate through services  
  - Use DI via `FunctionsStartup` or isolated worker model  
  - Avoid logic inside the function body  
- For Azure App Services / APIs:
  - Use `IHttpClientFactory` for all outbound calls  
  - Add retry policies with **Polly**  
  - Use managed identity when possible  
- For storage:
  - Prefer strongly typed clients (BlobClient, QueueClient, TableClient)  
  - Keep connection strings out of code

---

## **5. Logging & Observability**
- Use **Microsoft.Extensions.Logging** for all logs.  
- Log with structured logging:
  - `logger.LogInformation("User {UserId} created order {OrderId}", userId, orderId);`
- Include:
  - correlation IDs  
  - request IDs  
  - user context when available  
- Add **OpenTelemetry** instrumentation:
  - Traces for inbound/outbound calls  
  - Metrics for performance‑critical paths  
  - Exporters configured via environment variables

---

## **6. Error Handling**
- Use `try/catch` only where recovery is possible.  
- Throw meaningful exceptions — avoid generic `Exception`.  
- Never swallow exceptions silently.  
- For APIs, convert exceptions into consistent error responses.  
- For background workers, log and retry using policies.

---

## **7. Testing Guidelines**
- Use **xUnit** or **NUnit** consistently.  
- Prefer **unit tests** for business logic and **integration tests** for infrastructure.  
- Mock external dependencies using interfaces.  
- Keep tests deterministic — no real network calls.  
- Use test data builders for readability.

---

## **8. Front‑End / Next.js (if applicable)**
- Use **server components** by default.  
- Keep components small and composable.  
- Use **SWR** or React Query for data fetching when needed.  
- Prefer **TypeScript** everywhere.  
- Keep API calls in a dedicated `/lib/api` or `/services` folder.  
- Use environment variables via `.env.local` — never commit secrets.

---

## **9. Documentation & Comments**
- Add XML comments for public APIs and complex logic.  
- Keep README files up to date with:
  - setup instructions  
  - environment variables  
  - architecture overview  
- Use Markdown for architecture diagrams when helpful.

---

## **10. How Copilot Should Behave**
When generating code, Copilot should:
- Follow all rules in this file  
- Prefer clarity over cleverness  
- Match the existing project’s patterns  
- Use the folder structure already present  
- Avoid generating unused abstractions  
- Ask for missing context if the request is ambiguous  

When refactoring, Copilot should:
- Improve readability  
- Reduce duplication  
- Preserve behavior  
- Maintain architectural boundaries  

---

## **11. Project Structure (General Template)**

```
/src
  /Api
  /Application
  /Domain
  /Infrastructure

/tests
  /UnitTests
  /IntegrationTests

/docs
  architecture.md
  copilot-guidance.md
```

---

## **12. Security & Secrets**
- Never commit secrets.  
- Use environment variables or Azure Key Vault.  
- Validate all external input.  
- Sanitize logs to avoid leaking sensitive data.

---

## **13. Performance Expectations**
- Use async I/O everywhere.  
- Avoid blocking calls (`.Result`, `.Wait()`).  
- Cache expensive operations when appropriate.  
- Use pagination for large datasets.  
- Keep allocations low in hot paths.

---

## **14. Code Generation Requests**
When asked to generate code, Copilot should:
- Follow these guidelines  
- Use the project’s existing patterns  
- Keep code idiomatic and modern  
- Include necessary using statements  
- Provide explanations only when requested  

---


