
- Clean Architecture  
- Vertical Slice features  
- Future Blazor or Next.js front end  
- Infrastructure  
- Deployment automation  

# 🎮 **Dave’s Arcade — Folder Layout (Clean Architecture + Vertical Slice + Infra + Deploy)**

```plaintext
daves-arcade/
│
├── src/
│   ├── Api/                     # Minimal API project (endpoints, DI, middleware)
│   │   ├── Endpoints/
│   │   │   ├── Games/
│   │   │   └── Users/
│   │   ├── Extensions/
│   │   └── Program.cs
│   │
│   ├── Application/             # Vertical Slice features live here
│   │   ├── Games/
│   │   │   ├── Create/
│   │   │   ├── Update/
│   │   │   ├── Delete/
│   │   │   └── Get/
│   │   ├── Users/
│   │   │   ├── Register/
│   │   │   └── Login/
│   │   ├── Behaviors/           # Pipeline behaviors (logging, validation)
│   │   └── Common/              # Shared abstractions (interfaces, results)
│   │
│   ├── Domain/                  # Entities, enums, value objects
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   └── Events/
│   │
│   ├── Infrastructure/          # EF Core, repositories, external services
│   │   ├── Persistence/
│   │   │   ├── DbContext/
│   │   │   ├── Configurations/
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   └── DependencyInjection.cs
│   │
│   ├── Frontend/                # Placeholder for Blazor or Next.js later
│   │   ├── blazor/              # (optional future)
│   │   └── web/                 # (optional future)
│
├── infra/                       # Infrastructure-as-code, scripts, templates
│   ├── bicep/
│   ├── terraform/
│   ├── scripts/
│   └── README.md
│
├── deploy/                      # CI/CD pipelines, Dockerfiles, manifests
│   ├── docker/
│   │   ├── Api.Dockerfile
│   │   └── Frontend.Dockerfile
│   ├── github-actions/
│   ├── azure-pipelines/
│   └── README.md
│
├── tests/
│   ├── Api.Tests/
│   ├── Application.Tests/
│   └── Domain.Tests/
│
└── README.md                    # High-level explanation of architecture
```

---

# ⭐ Why this layout is perfect for your goals

### **1. Clean Architecture at the top level**
- Domain is pure  
- Application contains business logic  
- Infrastructure handles EF Core + external services  
- API is thin and clean  

### **2. Vertical Slice inside Application**
Each feature gets its own folder with:
- Request DTO  
- Response DTO  
- Handler  
- Validator  
- Mapping  

This keeps features cohesive and easy to navigate.

### **3. Future UI support**
You can drop in:
- **Blazor** under `/Frontend/blazor`  
- **Next.js** under `/Frontend/web`  


### **4. Infra + Deploy folders**

`infra/`  
- Bicep  
- Terraform  
- ARM  
- Scripts  

`deploy/`  
- Dockerfiles  
- GitHub Actions  
- Azure Pipelines  
- Container App manifests  
