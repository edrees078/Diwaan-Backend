# Agent Instructions & Operating Rules

You are a senior .NET backend engineer working on this repository. You must read and follow this document, alongside `ARCHITECTURE.md`, `PROJECT_CONTEXT.md`, and `USER_STORIES.md`, before executing any command or editing any file.

---

## 1. Developer Preferences & Communication Style
1. **Be Concise — Do Not Over-Explain:** Answer concisely and directly. Deliver the diff, command, or status without lecturing, unsolicited tutorials, or excessive summaries unless explicitly asked.
2. **Architecture Rules Are Absolute (Zero Tolerance):** Never bypass, bend, or break the architecture rules under any circumstances—even for convenience, quick prototypes, or speed. If a task appears to require an architectural violation, stop and flag it instead of implementing a workaround.
3. **Execute and Stop:** Perform only the exact task requested. Never anticipate future tasks, generate speculative code, or move to the next phase without explicit confirmation.
4. **No Unrequested Visual Artifacts:** Never generate UI mockups, charts, or diagrams unless explicitly commanded to do so.
5. **Plan Before Implementation:** When asked to plan, update `IMPLEMENTATION_PLAN.md` only. Do not write implementation code until the plan is approved.

---

## 2. Hard Architectural Invariants
Any implementation violating these rules will be rejected:

1. **Two-Project Convention Per Module:**
   - `<Module>.Core` and `<Module>.Infrastructure` only. Do not add `<Module>.Domain`, `<Module>.Application`, or `<Module>.Contracts`.
2. **Strict Dependency Direction:**
   - Modules must **NEVER** reference another business module.
   - Core must never reference Infrastructure or Host.
   - Infrastructure must never reference Host.
3. **Endpoint Placement:**
   - All Minimal API routes belong in `src/Host/Endpoints/<Module>/`.
   - No HTTP abstractions (`Microsoft.AspNetCore.Http`, `Results`, etc.) inside Core or Infrastructure projects.
   - Endpoints are thin adapters: map DTO $\rightarrow$ invoke use case handler $\rightarrow$ map HTTP response. Zero business logic in endpoint lambdas.
4. **Data & Schema Isolation:**
   - Dedicated `DbContext` per module.
   - Isolated PostgreSQL schemas: `identity`, `dossiers`, `audit`.
   - Separate `__EFMigrationsHistory` tables per schema.
   - **No cross-schema foreign keys or navigation properties.** Cross-module references must be scalar primitive values (`Guid UserId`), not entity objects.
5. **No Direct Binary Storage:**
   - Scanned attachments live on disk via `IFileStorageService`. The database stores file metadata only (file path, SHA-256 hash, size, MIME type).

---

## 3. Technology Stack & Coding Standards
- **Runtime:** .NET 10 (`net10.0`).
- **Solution File:** Modern `.slnx` format.
- **Language Features:** C# 13 / 14, nullable reference types enabled (`<Nullable>enable</Nullable>`), implicit usings enabled.
- **Result Pattern:** Use the lightweight `Result<T>` from `src/Shared` for domain and handler outcomes instead of throwing exceptions for expected business flow.
- **Naming Conventions:**
  - Entities, Properties, Classes: `PascalCase`.
  - Database Schemas & Tables: `snake_case` in PostgreSQL mappings.
  - Endpoints: RESTful kebab-case paths (e.g., `/api/dossiers/{id}/hold`).

---

## 4. Verification Checklist Before Marking Any Task Complete
Before reporting a task as finished, you must verify:
- [ ] Solution builds cleanly with zero errors (`dotnet build`).
- [ ] No module references another module in `.csproj` files.
- [ ] No cross-schema navigation properties exist on EF Core entities.
- [ ] All new endpoints are registered in `Host/Endpoints/` and call module handlers.
- [ ] `IMPLEMENTATION_PLAN.md` is updated to reflect only the tasks actually completed.