Perfect — let’s extend your setup with the next pieces that pair naturally with `copilot-guidance.md` so every repo you touch feels consistent, senior‑level, and AI‑friendly.

Below are the next two components you’ll want in *every* project:

- **`architecture.md`** — a clean, reusable architecture overview  
- **VS Code Workspace Instructions** — so Copilot automatically follows your rules without you repeating them  

Both are written to be universal, but still aligned with how *you* build systems.

---

# **architecture.md (drop‑in template)**

## **1. Overview**
This project follows a clean, layered architecture designed for clarity, testability, and long‑term maintainability. The goal is to keep business logic isolated, infrastructure swappable, and the API surface thin and predictable.

The architecture is intentionally simple, avoiding unnecessary abstractions while still supporting enterprise‑grade patterns.

---

## **2. Layer Breakdown**

### **Domain Layer**
- Contains core business entities, value objects, and domain logic  
- No dependencies on other layers  
- Pure C# — no infrastructure concerns  
- Should remain stable over time  

### **Application Layer**
- Contains use cases, handlers, validators, and interfaces  
- Coordinates domain logic  
- Uses **MediatR** for commands/queries (when appropriate)  
- Uses **FluentValidation** for input validation  
- Depends only on Domain  

### **Infrastructure Layer**
- Contains implementations of interfaces defined in Application  
- Handles:
  - Data access  
  - External APIs  
  - Azure services  
  - Logging  
  - Caching  
- Uses DI to register services  
- Depends on Application and Domain  

### **API Layer (or Azure Functions)**
- Thin entry point  
- No business logic  
- Maps requests → Application layer  
- Maps responses → DTOs  
- Handles authentication/authorization  
- Provides OpenAPI/Swagger when applicable  

---

## **3. Cross‑Cutting Concerns**

### **Logging**
- Use `ILogger<T>` everywhere  
- Structured logs only  
- Include correlation IDs  

### **OpenTelemetry**
- Tracing for inbound/outbound calls  
- Metrics for performance‑critical paths  
- Exporters configured via environment variables  

### **Configuration**
- All settings come from:
  - `appsettings.json`  
  - environment variables  
  - Azure Key Vault  
- No hard‑coded values  

---

## **4. Folder Structure**

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

## **5. Data Access (if applicable)**
- Use EF Core or Dapper depending on project needs  
- Repositories implement interfaces from Application  
- No direct DB calls from API or Domain  
- Migrations live in Infrastructure  

---

## **6. API Conventions**
- Use DTOs for all inbound/outbound models  
- Return consistent HTTP responses  
- Keep controllers/functions thin  
- Use minimal APIs when appropriate  

---

## **7. Error Handling**
- Convert exceptions into standardized error responses  
- Use retry policies for transient failures  
- Never swallow exceptions silently  

---

## **8. Testing Strategy**
- Unit tests for Application + Domain  
- Integration tests for Infrastructure + API  
- Mock external dependencies  
- No real network calls in tests  

---

## **9. Deployment**
- CI/CD via GitHub Actions or Azure DevOps  
- Build → Test → Publish → Deploy  
- Use environment‑specific configuration  
- Prefer containerized deployments when possible  

---

# **VS Code Workspace Instructions (paste into `.vscode/settings.json`)**

These make Copilot behave consistently across all your repos.

```json
{
  "github.copilot.chat.workspaceInstructions": [
    "Follow the rules in docs/copilot-guidance.md.",
    "Follow the architecture described in docs/architecture.md.",
    "Use async/await everywhere.",
    "Use dependency injection for all services.",
    "Keep controllers and Azure Functions thin.",
    "Use DTOs for all inbound/outbound models.",
    "Use MediatR and FluentValidation when appropriate.",
    "Use structured logging with ILogger<T>.",
    "Add OpenTelemetry instrumentation for inbound/outbound calls.",
    "Match the existing folder structure and coding style."
  ]
}
```

This gives Copilot a **persistent memory** of how you want code generated — without you having to repeat yourself.

---

# **If you want, I can generate next:**

### **Option A — A full repo template**
- folder structure  
- starter projects  
- DI setup  
- logging  
- OpenTelemetry  
- sample API  
- sample tests  

### **Option B — A Copilot “starter prompt” you can paste into Chat**
A reusable prompt that sets the tone for every coding session.

### **Option C — A Next.js version of the same architecture**
If you want your front‑end repos to follow the same discipline.

### **Option D — A GitHub Actions CI/CD pipeline**
Container build, test, publish, deploy.

Which one do you want next, Dave?
