# Agent Execution Prompts

## Prompt 1: Phase 1 Planning & Plan Population
> "Read `ARCHITECTURE.md`, `PROJECT_CONTEXT.md`, and `USER_STORIES.md`.
> Review the provided Figma link/specifications: [INSERT FIGMA LINK HERE].
> Your task is to update `IMPLEMENTATION_PLAN.md`:
> 1. Complete Phase 1 Task 1.1 and Task 1.2.
> 2. Detail Phase 3 and all subsequent phases down to individual atomic tasks (entities, migrations, handlers, endpoints, integration tests).
> 3. Ensure no architecture rules (schema isolation, two-project rule, no inter-module references) are violated.
> DO NOT write any application code or create project files yet. Update `IMPLEMENTATION_PLAN.md` and stop for my review."

## Prompt 2: Phase 2 Scaffolding & Setup (Run after approving Plan)
> "The implementation plan is approved. Execute Phase 2 of `IMPLEMENTATION_PLAN.md`:
> 1. Create the solution structure and projects matching `ARCHITECTURE.md`.
> 2. Configure project references and package dependencies.
> 3. Implement the shared kernel in `Shared` and configure database contexts with schema separation.
> Stop and report verification once Phase 2 is complete. Do not begin Phase 3 without confirmation."