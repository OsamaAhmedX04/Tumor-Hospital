# 🏥 Tumor Hospital — Backend (ASP.NET Core / .NET 8)

A comprehensive, developer-friendly README for the Tumor Hospital backend. This document explains the solution structure, every major feature, all libraries and integrations, deployment & development guidance, and a deep technical dive into the appointment system and the race-condition mitigation strategy implemented in this repository.

---

## 🚀 Quick summary

Tumor Hospital is a .NET 8 backend that exposes a RESTful API to manage hospital resources and workflows focused on oncology care: patients, practitioners, appointments, files, notifications, background jobs, and operational health. It follows a layered architecture (Domain / Application / Infrastructure / WebAPI) to keep domain logic isolated and testable.

---

## 📁 Solution layout

- TumorHospital.WebAPI — API entrypoint, controllers, DI setup, middlewares, Swagger
- TumorHospital.Application — Application services, use-cases, DTOs, validation, AutoMapper profiles
- TumorHospital.Infrastructure — EF Core DbContext, repositories, identity stores, third-party integrations, Hangfire jobs
- TumorHospital.Domain — Domain entities, value objects, domain contracts

---

## ⭐ Feature catalog (explained)

Each feature below includes what it does, why it exists, and key implementation notes.

1. Authentication & Authorization 🔐
   - What: User registration, login, JWT-based tokens, role-based access (Admin, Doctor, Patient).
   - Why: Secure endpoints and restrict sensitive operations (e.g., appointment approvals, patient records).
   - Implementation notes: ASP.NET Core Identity stores users/roles in EF Core; JWT tokens issued by the API using System.IdentityModel.Tokens.Jwt; middleware enforces Bearer token authentication and role policies.

2. User management (Patients & Practitioners) 👥
   - What: CRUD endpoints and profile management for patients and practitioners (doctors, nurses).
   - Why: Centralized identity + domain profile for all actors.
   - Implementation notes: Application layer DTOs, FluentValidation rules, AutoMapper profiles; Domain entities persist via EF Core.

3. Appointment system 📅 (deep dive later)
   - What: Book, view, reschedule, cancel, accept/reject appointments, availability checks, reminders.
   - Why: Core clinical workflow—matching patients and practitioners at times.
   - Implementation notes: EF Core entities for Appointment, PractitionerSchedule, and AppointmentSeries. Hangfire schedules reminder emails. Concurrency handled using DB constraints + optimistic concurrency + transactional flows.

4. FAQs and content management ❓
   - What: CRUD endpoints to manage Frequently Asked Questions and site content.
   - Why: Provide static help content to patients and staff.
   - Implementation notes: Simple CRUD in Application layer with validators and Admin-protected APIs.

5. File storage & media handling 📎
   - What: Upload and download patient files, documents and images.
   - Why: Medical documents and imaging must be stored and accessible alongside records.
   - Implementation notes: Supabase Storage SDK used to push files to Supabase; files references persist in DB. Service role keys kept server-side.

6. Background jobs & scheduling ⏱️
   - What: Async jobs (email reminders, cleanup, report generation) and scheduled tasks.
   - Why: Reliable retries, scheduling and offloading long-running tasks.
   - Implementation notes: Hangfire configured with SQL Server storage; background workers perform reminder sends, batch exports, and other async tasks.

7. Email & notifications ✉️
   - What: Transactional emails (verification, reminders) and integration points for SMS/push.
   - Why: Keep patients and staff informed about bookings and system events.
   - Implementation notes: SendGrid SDK used to send emails; retries and health checks integrated.

8. Observability & diagnostics 🔍
   - What: Structured logs, health checks, and API documentation.
   - Why: Production troubleshooting, uptime monitoring, and API discoverability.
   - Implementation notes: Serilog with file sink; HealthChecks (SQLServer, SendGrid); Swagger/Swashbuckle with annotations.

9. Input validation & mapping ✅
   - What: Validation rules for DTOs and mapping between DTOs and domain models.
   - Why: Keep controllers thin and validate inputs before business logic executes.
   - Implementation notes: FluentValidation used in pipeline; AutoMapper profiles centralize mapping logic.

10. Security & compliance 🔒
    - What: Role-based access, token expiration, secrets via environment or secret store.
    - Why: Protect PHI and conform to security best-practices.
    - Implementation notes: Keep secrets out of repo; consider encryption and audit logging in production.

---

## 🧩 Libraries & technologies (explicit)

- Framework: .NET 8 (net8.0), ASP.NET Core Web API
- Identity & Auth: Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.AspNetCore.Authentication.JwtBearer, System.IdentityModel.Tokens.Jwt
- ORM: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools
- Querying: LinqKit.Microsoft.EntityFrameworkCore
- Validation: FluentValidation.AspNetCore
- Mapping: AutoMapper.Extensions.Microsoft.DependencyInjection
- Background jobs: Hangfire
- Email: SendGrid, AspNetCore.HealthChecks.SendGrid
- File storage: Supabase, Supabase.Storage
- Logging: Serilog.AspNetCore, Serilog.Settings.Configuration, Serilog.Sinks.File
- API docs: Swashbuckle.AspNetCore, Swashbuckle.AspNetCore.Annotations
- Utilities: Microsoft.AspNetCore.WebUtilities, Newtonsoft.Json

---

## 🔗 Integrations & where to find them in the codebase

- SendGrid (email delivery): configured in Infrastructure; used by Notification services and Hangfire jobs.
- Supabase Storage: used by FileStorage service in Infrastructure.
- Hangfire: configured in WebAPI startup to run background jobs and expose dashboard (protect dashboard with auth).
- HealthChecks: hooks for SQL Server & SendGrid are registered in Infrastructure and surfaced by WebAPI.

---

## 📘 Design & coding conventions

- Layered Clean Architecture: Domain → Application → Infrastructure → WebAPI
- Use DTOs in Application layer; do not expose EF entities directly from controllers.
- All public API models validated via FluentValidation.
- AutoMapper maps between DTOs and Domain models.
- Use Serilog for structured logs; wire configuration in appsettings and environment.

---

## 🛠️ Development & local setup (step-by-step)

1. Prerequisites
   - Install .NET 8 SDK
   - SQL Server (LocalDB or container)
   - Optional: Redis (if you choose to enable distributed locks in the future)

2. Clone & restore

```powershell
git clone https://github.com/OsamaAhmedX04/Tumor-Hospital.git
cd "Tumor Hospital"
dotnet restore
```

3. Configure connection and secrets
   - Copy a template appsettings.Development.json (create one) and set ConnectionStrings__DefaultConnection, Jwt__Key, SendGrid__ApiKey, Supabase keys.

4. Database

```powershell
dotnet build
dotnet ef database update --project TumorHospital.Infrastructure --startup-project TumorHospital.WebAPI
```

5. Run API

```powershell
dotnet run --project TumorHospital.WebAPI
```

6. Visit
   - Swagger: https://localhost:{port}/swagger
   - Hangfire dashboard (if enabled): https://localhost:{port}/hangfire — secure this in dev/prod

---

## 🧪 Testing strategy

- Unit tests for domain and application layer logic (esp. scheduling and validation).
- Integration tests for DB transactions and appointment flows using an in-memory DB or a local SQL Server instance.
- End-to-end tests for API flows (book -> accept/reject -> remind) using test accounts.

---

## 💡 Appointment system — deep technical dive (large section)

This is intentionally detailed because appointment scheduling is both critical and complex. Read it fully before making schema or concurrency changes.

Goals
- Provide correct availability information
- Prevent double-bookings
- Allow accept/reject workflows for practitioners
- Support rescheduling, cancellations, reminders, and recurring series
- Be resilient under concurrent user operations

Core models
- Appointment { Id, PatientId, PractitionerId, ServiceTypeId, StartUtc, EndUtc, Status, RowVersion }
- PractitionerSchedule { PractitionerId, DayOfWeek, StartTimeLocal, EndTimeLocal, IsRecurring }
- AppointmentSeries (optional) for recurring bookings

Key constraints and invariants
- Appointment time ranges must not overlap for the same practitioner
- Appointments are stored in UTC
- Status can be Pending, Confirmed, Rejected, Cancelled, Completed

Common race conditions
- Two clients try to book the same practitioner + overlapping slot simultaneously -> double-booking
- Practitioner accepts/rejects while another client attempts reschedule -> inconsistent state
- Background worker (reminder) fires while appointment is being cancelled -> stale notification

Strategy implemented in this repository (how it is solved)
1) Defensive database constraints ✅
   - A unique or exclusion constraint on practitioner + timeslot is the strongest guard. For SQL Server, a deterministic approach is to enforce non-overlap via an indexed computed column and constraints or via a trigger + validation, but the simplest practical strategy is to rely on a uniqueness model for discrete slots (e.g., slot-based scheduling) or use an appointment table with start/end times and a non-overlap check in a transaction.
   - Example: If the system uses fixed discrete slots (e.g., 15-minute slots), store SlotUtcStart and use unique constraint on (PractitionerId, SlotUtcStart).

   SQL example (discrete slots):
   ```sql
   CREATE UNIQUE INDEX IX_Appointment_Practitioner_Slot
   ON Appointments(PractitionerId, SlotUtcStart);
   ```

2) Application-level transactional booking ✅
   - Booking endpoints perform a read-check-then-insert inside a serializable-safe transaction (or repeatable-read) where possible.
   - In EF Core this looks like: begin transaction -> check for overlapping appointments -> insert appointment -> commit. If commit fails due to DB constraint, return 409 Conflict and let client retry with updated availability.

3) Optimistic concurrency for edits ✅
   - Appointments include a RowVersion (byte[] / timestamp) that EF Core treats as a concurrency token. When updating Accept/Reject, the API checks the current rowversion to avoid blind overwrites.
   - If EF throws DbUpdateConcurrencyException, the API returns 409 Conflict and the client can re-fetch state.

4) Application-level locking for acceptance/rejection (practical approach used here) ✅
   - Accept/Reject operations are sensitive: they change status atomically and may trigger notifications.
   - Implementation approach in this repo:
     - Use a short-lived SQL-based application lock (sp_getapplock) per Appointment.Id or Practitioner.Id where supported, or a transaction combined with a SELECT ... FOR UPDATE pattern when using a DB that supports it.
     - The code attempts to acquire the lock, re-check the appointment status inside the lock/transaction, then perform the state change and commit. If lock cannot be acquired quickly, return 423 Locked or retry a few times.

   - Example pseudocode for Accept endpoint (C# / EF Core style):
   ```csharp
   using var tx = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
   var appointment = await _dbContext.Appointments
        .Where(a => a.Id == id)
        .FirstOrDefaultAsync();

   if (appointment == null) return NotFound();
   if (appointment.Status != Pending) return Conflict("Already processed");

   appointment.Status = Confirmed;
   appointment.RowVersion = appointment.RowVersion; // keep concurrency token

   try {
       await _dbContext.SaveChangesAsync();
       await tx.CommitAsync();
   }
   catch (DbUpdateConcurrencyException) {
       // Someone else modified the appointment; return 409
       return Conflict("Appointment changed, refresh and retry");
   }
   ```

   - If stronger cross-process locks are needed, adopt sp_getapplock in SQL Server or Redis locks (RedLock) for a distributed environment.

5) Idempotency and client tokens ✅
   - For booking endpoints, support an Idempotency-Key header; server stores request token -> result mapping in a short-lived table to prevent duplicate bookings when clients retry.

6) Background jobs & eventual consistency ✅
   - Hangfire handles background reminder emails. Jobs check appointment status right before sending and will skip if appointment is Cancelled/Rejected.
   - Jobs are idempotent: they track their last run and skip duplicates.

7) Retry & conflict resolution UX ✅
   - If a booking/accept request hits a conflict, the API returns 409 with a helpful payload describing current availability or the conflicting appointment, and the client UI can surface a helpful message to the user.

Why this hybrid approach?
- Relying only on DB constraints gives correctness but poor UX (clients get a generic constraint violation).
- Relying only on app-level locks requires reliable distributed locks for multi-instance deployments.
- Combining DB-level constraints with optimistic concurrency and short-lived locks yields both correctness and predictable client behavior.

Code pointers (where to look)
- Appointment entity & DbContext: TumorHospital.Infrastructure → Entities / Data
- Booking & Accept/Reject endpoints: TumorHospital.WebAPI → Controllers → AppointmentsController
- Booking business logic & concurrency helpers: TumorHospital.Application → Services → AppointmentService
- Hangfire job implementations: TumorHospital.Infrastructure → Jobs

---

## 🔧 Example API flows (with curl)

1) Book appointment (client)

```bash
curl -X POST "https://localhost:{port}/api/appointments" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"practitionerId":"...","startUtc":"2026-07-01T08:00:00Z","endUtc":"2026-07-01T08:30:00Z","serviceTypeId":"..."}'
```

Successful responses: 201 Created with appointment DTO. If conflict: 409 with current overlapping appointments or suggested slots.

2) Practitioner accepts (server-side concurrency safe)

```bash
curl -X POST "https://localhost:{port}/api/appointments/{id}/accept" \
  -H "Authorization: Bearer {practitioner-token}"
```

Responses
- 200 OK on success
- 409 Conflict if already processed
- 423 Locked if lock acquisition failed (temporary)

---

## ✅ Best practices & recommendations

- Keep secrets out of source control. Use environment variables or secret managers.
- Run Hangfire and the WebAPI with a single SQL storage backend or use a dedicated storage for jobs if scaling horizontally.
- Protect the Hangfire dashboard and admin endpoints.
- Monitor and alert on failed jobs and email send failures.
- For extreme scale, move to slot-based booking with precomputed availability caches and use a central locking service (Redis) or event-sourced booking pipeline.

---

## 🧭 Troubleshooting

- 409 on booking: fetch latest availability and retry with a different slot.
- Reminder emails not sending: check Hangfire jobs, SendGrid API key, and health checks.
- Concurrency exceptions on Update: surface to client and implement retry logic or refresh UI.

---

## ♻️ Contribution & code hygiene

- Add tests for all new scheduling rules and concurrency scenarios.
- Keep controllers thin — move business policies to Application services.
- Use feature branches and open PRs with clear descriptions.

---

## 📄 License
Add a LICENSE file (e.g., MIT) if you want to open-source this repository. Otherwise include your internal license terms.

---

## ✨ Final notes

This README is intentionally opinionated about the appointment system. The codebase includes the patterns described above — look for the AppointmentService and the Accept/Reject implementations in the Application and Infrastructure layers to see the concurrency patterns in use. If you want, I can also add a focused design doc or sequence diagrams for the booking flows, or extract the Accept/Reject logic into a small, self-contained sample demonstrating the lock + transaction pattern.
