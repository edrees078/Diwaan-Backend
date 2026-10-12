# Diwan Backend — Local Development & Operations Guide

This guide documents local development setup, infrastructure services, configuration, and EF Core migration commands.

---

## 1. Prerequisites

- **.NET 10 SDK** (verified with installed SDK `10.0.200-preview.0.26103.119` and runtime `10.0.2`)
- **Docker & Docker Compose** (for local PostgreSQL and MinIO)
- **`dotnet-ef` global tool** (optional, for managing migrations):
  ```powershell
  dotnet tool install --global dotnet-ef
  ```

---

## 2. Configuration & Environment Variables

### `.env` File
Copy [`.env.example`](file:///c:/Users/mosa/OneDrive/Desktop/alqalam/.env.example) to `.env` if you wish to override default credentials locally:
```powershell
Copy-Item .env.example .env
```
> [!NOTE]
> `.env` is ignored by Git and must never contain production secrets.

### Connection Strings & Storage Options
Configuration is defined in [`src/Host/appsettings.json`](file:///c:/Users/mosa/OneDrive/Desktop/alqalam/src/Host/appsettings.json):
```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Port=5432;Database=alqalam;Username=alqalam_dev;Password=alqalam_dev_password"
  },
  "Storage": {
    "InternalEndpoint": "http://localhost:9000",
    "PublicEndpoint": "http://localhost:9000",
    "Region": "us-east-1",
    "BucketName": "alqalam-dataset",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin"
  }
}
```

---

## 3. Running the API Locally

Run the Host natively using the .NET CLI:
```powershell
dotnet run --project src/Host/AlQalam.Host.csproj --launch-profile http
```

### Health Endpoints
- **Liveness Probe**: `GET http://localhost:5000/health/live`
  - Returns `200 OK` `{ "status": "live" }` whenever the host process is running.
- **Readiness Probe**: `GET http://localhost:5000/health/ready`
  - Verifies connectivity to the database.
  - Returns `200 OK` `{ "status": "ready" }` when dependencies are healthy.
  - Returns `503 Service Unavailable` `{ "status": "unhealthy" }` when dependencies are down, without leaking sensitive internals or credentials.

### OpenAPI Specification
- When running in Development mode:
  - `GET http://localhost:5000/openapi/v1.json`

---

## 4. Running Tests

Run the complete test suite:
```powershell
dotnet test AlQalam.slnx
```

Tests include:
- Result/Error primitives and failure type assertions.
- ProblemDetails HTTP status mapping and error sanitization.
- DbContext schema isolation (`users`, `memorization`, `dataset`).
- StorageOptions configuration binding.
- Health endpoint liveness and readiness probes.

---

## 5. Entity Framework Core & Database Migrations

### Architecture Rules
- Each module has its own `DbContext` and its own schema:
- Avoid cross-module foreign key navigation properties or cross-module database joins.
- **Production Migrations**: Run migrations as a controlled deployment step (e.g. via `dotnet ef database update` or generated idempotent SQL scripts). **Do not** run concurrent `Database.Migrate()` on every API instance startup.

### Generating Migrations (Phase 3+)
When entities are added in future phases, generate migrations per context



### Applying Migrations Locally



---

## 7. Dependency Licenses & Compatibility

All dependencies are officially maintained, compatible with .NET 10, and use standard permissive open-source licenses:
- `Microsoft.EntityFrameworkCore` 10.0.12 (MIT)
- `Microsoft.EntityFrameworkCore.Relational` 10.0.12 (MIT)
- `Microsoft.EntityFrameworkCore.Design` 10.0.12 (MIT)
- `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3 (PostgreSQL License)
- `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` 10.0.12 (MIT)
- `Microsoft.AspNetCore.OpenApi` 10.0.12 (MIT)
- `xUnit` 2.9.3 (Apache 2.0)
- `Microsoft.NET.Test.Sdk` 18.10.1 (Microsoft Pre-Release/Standard Software License)
- `coverlet.collector` 6.0.4 (MIT)
