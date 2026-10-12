---

### File 2: `PROJECT_CONTEXT.md`

```markdown
# Project Context: Government File Tracking & Custody System

## Background
The target environment is a government municipal/provincial department currently relying entirely on physical paper files. Prior to this system, dossiers were routed manually between desks, leading to lost documents, unrecorded delays, and complete lack of accountability.

This system serves as an on-premise, intranet-based digital twin and transit ledger. It ensures every document that enters from the Diwan is digitized, assigned an immutable tracking code, signed for at every desk transition, and tracked through its physical handoff.

## Primary Stakeholders & Roles
1. **Manager (المدير):**
   - Receives physical folders from the Diwan.
   - Registers metadata, scans attachments, and assigns an Engineer.
   - Holds administrative authority to configure request types and reset staff passwords locally.
2. **Engineer (المهندس):**
   - Conducts technical reviews.
   - Can optionally attach engineering documents and sign off directly to an Employee.
   - Can place a document on formal external hold while coordinating with outside organizations.
3. **Employee (الموظف):**
   - Receives reviewed files from the Engineer.
   - Verifies the dossier, signs acceptance, and logs physical handoff to two designated external reviewers.

## Core Domain Rules
- **No External Dependencies:** The system operates in an isolated intranet environment. No external authentication providers, external notification services, or third-party cloud services are permitted.
- **Step-by-Step Custody Handshake:** A file cannot transition without explicit action and digital signature from the current assignee.
- **Bidirectional Routing:** At each step, an assignee may:
  - **Accept & Forward** to the next designated role.
  - **Reject & Return** to the immediately preceding actor, requiring an explicit, non-empty rejection justification.
- **Append-Only Auditing:** Every assignment, approval, hold, return, and signature must be permanently logged with exact timestamps and actor identifiers. Audit entries cannot be modified or deleted.