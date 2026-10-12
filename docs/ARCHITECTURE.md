# Architecture Specification: Government Document Routing System

## 1. System Shape: Modular Monolith
The application is structured as an ASP.NET Core Modular Monolith deployed as a single Host executable. Physical boundaries enforce modular separation.

### Bounded Contexts (Modules):
1. **Identity**: Local user accounts, authentication tokens/sessions, role authorization (`Manager`, `Engineer`, `Employee`), manager-directed credential resets.
2. other modules will be determend later understanding the full context
### Physical Project Structure:
Every business module is restricted to exactly two production projects:
- `<Module>.Core`: Domain models, enums, value objects, domain logic, use-case handlers/commands/queries, and module-specific abstractions.
- `<Module>.Infrastructure`: EF Core `DbContext`, entity mappings, database migrations, disk file storage adapters, and external integration implementations.

Supporting non-module projects:
- `Host`: ASP.NET Core web host, DI composition, middleware, configuration, and Minimal API endpoint route registrations.
- `Shared`: Kernel types, common result objects (`Result<T>`), base abstractions, and shared contract primitives. Must remain deliberately minimal.

Test projects are excluded from the two-project convention.

---

## 2. Dependency Inversion & Reference Rules
- `Shared` has zero dependencies on any business module or the Host.
- `<Module>.Core` references `Shared` only. It must not reference any Infrastructure project, the Host, or other module Core projects.
- `<Module>.Infrastructure` references its own `<Module>.Core` and `Shared`. It must never reference `Host` or other module projects.
- `Host` references all `<Module>.Core`, `<Module>.Infrastructure`, and `Shared` projects for DI composition.
- **Strict Rule:** Direct references between business modules (e.g., `Dossiers.Core` -> `Identity.Core`) are strictly forbidden.
- **Cross-Module Coordination:** Must occur strictly via public contract interfaces defined in `Shared` or mediated through the Host layer.

---

## 3. API & Endpoint Ownership
- Minimal API endpoints live exclusively in `Host/Endpoints/<Module>/`.
- `Host/Program.cs` registers endpoint mapping extensions.
- Infrastructure and Core projects must not reference ASP.NET Core Web abstractions or map HTTP routes.
- Endpoints serve strictly as protocol adapters:
  1. Map HTTP request payload to module command/query contract.
  2. Dispatch contract to module application handler.
  3. Map execution result to standard HTTP responses (`Results.Ok`, `Results.BadRequest`, `Results.NotFound`).
- No business logic or domain validations are permitted inside endpoint lambdas.

---

## 4. Data Storage & Schema Isolation
- Single PostgreSQL database instance.
- Dedicated `DbContext` per business module.
- Strict PostgreSQL schema separation:
  - `identity.*`
  - `dossiers.*`
  - `audit.*`
- Each module configures its dedicated migrations history table:
  ```csharp
  options.UseNpgsql(connectionString, x => 
      x.MigrationsHistoryTable("__EFMigrationsHistory", "schema_name"));