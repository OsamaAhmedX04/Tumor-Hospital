# Tumor Hospital — Backend (ASP.NET Core / .NET 8)

Comprehensive README for the Tumor Hospital backend. This document summarizes features, libraries and technologies used, integrations, deployment and development guidance, and a detailed section on the complexity of implementing a production-ready appointment system.

Projects in this solution
- TumorHospital.WebAPI — ASP.NET Core Web API (entry point)
- TumorHospital.Application — Application layer (DTOs, services, validation, mapping)
- TumorHospital.Infrastructure — Infrastructure concerns (EF Core, Identity, external integrations, background jobs)
- TumorHospital.Domain — Domain models and core abstractions

Table of contents
- Features
- Libraries & Technologies
- Integrations
- Appointment system: complexity and design considerations
- Development & local setup
- Deployment & operational notes
- Contribution

## Features
- Authentication & Authorization: ASP.NET Core Identity + JWT bearer tokens, role-based access (Admin, Doctor, Patient).
- User management: registration, profile management, role assignment.
- Patients & Doctors: CRUD APIs and profile management.
- Appointment management: create, read, update, cancel, availability checks, reminders (background jobs).
- FAQs management endpoints and content administration.
- File storage: upload/download support (Supabase Storage integration configured in Infrastructure).
- Background processing: Hangfire for scheduled jobs and asynchronous tasks (reminders, batch jobs, cleanup).
- Email notifications: SendGrid integration for transactional emails (verification, reminders, alerts).
- Health checks: readiness/liveness endpoints and UI integration.
- Logging and observability: Serilog with file sink, structured logging.
- API documentation: Swagger with annotations.
- Validation: FluentValidation pipeline for DTOs and API inputs.
- Mapping: AutoMapper profiles for DTO ↔ domain transformations.

## Libraries & technologies used
- Platform: .NET 8 (net8.0), ASP.NET Core Web API
- Authentication & identity: Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.AspNetCore.Authentication.JwtBearer, System.IdentityModel.Tokens.Jwt
- Persistence: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, LinqKit for complex expressions
- Background jobs: Hangfire
- Email: SendGrid + AspNetCore.HealthChecks.SendGrid
- File storage: Supabase, Supabase.Storage
- Validation & mapping: FluentValidation.AspNetCore, AutoMapper.Extensions.Microsoft.DependencyInjection
- Logging & diagnostics: Serilog.AspNetCore, Serilog.Settings.Configuration, Serilog.Sinks.File
- API docs: Swashbuckle.AspNetCore, Swashbuckle.AspNetCore.Annotations
- Misc: Microsoft.AspNetCore.WebUtilities, Newtonsoft.Json

## Integrations
- SendGrid: transactional email delivery for account verification, appointment reminders, admin alerts. Use service API key stored in configuration or a secrets store.
- Supabase Storage: used for storing uploaded documents, patient imaging, or other files. Service role keys must be kept secret and server-side operations limited.
- Hangfire: persistent background job processing using SQL Server storage or other supported storage. Dashboard typically exposed at /hangfire (protect with auth).
- HealthChecks: SQL Server and SendGrid integrations for health probes; HealthChecks UI can be enabled for monitoring.

## Appointment system — complexity and design considerations
Implementing a production-grade appointment system in a hospital context is one of the most complex subsystems. Below are key facets and recommendations.

1) Domain modeling and resources
- Model entities: Appointment, Practitioner (doctor), Patient, Room, ServiceType, and optionally Equipment.
- Availability model: working schedules, recurring availabilities, breaks, holidays, leave, and exceptions.

2) Slot generation and querying
- Generate possible slots from practitioners' schedules and service durations.
- Provide efficient availability queries (date range + filters) and pagination.
- Cache generated availability where appropriate but invalidate on schedule changes or bookings.

3) Concurrency and double-booking
- Use transactional booking flows. Prefer database-level constraints for critical uniqueness (e.g., unique index on practitioner+timeslot) to guard against race conditions.
- Implement optimistic concurrency tokens for appointment edits.
- For high-concurrency scenarios, consider distributed locks (Redis, advisory locks) or a single-writer job approach.

4) Time zones & DST
- Store all times in UTC in the DB. Convert to and from local time only at the API boundary or UI.
- Carefully test DST transitions and recurring appointments that cross DST boundaries.

5) Recurring appointments & exceptions
- Model series and exception items separately so edits/cancellations can apply to a single occurrence or the whole series.

6) Rescheduling and cancellations
- Expose idempotent endpoints and use idempotency keys for client retries.
- Maintain audit trail for changes and trigger compensating actions (e.g., cancel reminders).

7) Notifications & reminders
- Use Hangfire to schedule reminder jobs. Persist reminder state and track delivery attempts.
- Implement retry and dead-lettering for failed notifications.

8) Integration with external calendars
- Provide optional iCal export or two-way sync (Google Calendar / Exchange) using secure OAuth flows.

9) Testing & validation
- Extensive unit tests for scheduling logic, integration tests hitting DB transactions, and end-to-end tests for booking flows.

10) Security & compliance
- Enforce least-privilege for file storage and integrations. Apply encryption at rest for sensitive health data as required by regulations.

Trade-offs & performance
- Real-time availability computation is costly; use caching and incremental updates when needed.
- Strong consistency (preventing double-booking) requires careful design; optimistic approaches are easier but can lead to conflicts.

## Development & local setup
### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB, Docker container, or remote instance)
- (Optional) Redis for distributed locking, if used

### Environment variables / recommended settings
- ConnectionStrings__DefaultConnection — SQL Server connection string
- Jwt__Issuer, Jwt__Audience, Jwt__Key — JWT settings
- SendGrid__ApiKey — SendGrid API key
- Supabase__Url, Supabase__Key — Supabase configuration for storage

### Common commands
- dotnet restore
- dotnet build
- dotnet ef database update --project TumorHospital.Infrastructure --startup-project TumorHospital.WebAPI
- dotnet run --project TumorHospital.WebAPI

### Running Hangfire dashboard
- Dashboard usually exposed under /hangfire; secure the endpoint with authentication and role checks.

## Operational considerations
- Secrets: use an external secret manager (Azure Key Vault, AWS Secrets Manager) in production.
- Logging: centralize logs (Seq, Elasticsearch, Application Insights). Serilog is configured with a file sink by default.
- Monitoring: enable health checks and integrate with your monitoring system for alerts.
- Backups: schedule DB backups and test restore procedures.

## Contribution
- Follow the layered architecture: Domain → Application → Infrastructure → WebAPI.
- Add unit and integration tests for domain logic (especially appointment scheduling and concurrency scenarios).
- Keep DI registrations and environment-specific configuration in the WebAPI project.

## License
- Add a LICENSE file to the repo to declare licensing (e.g., MIT) if open-sourcing. Otherwise include internal licensing terms.

## Next steps
- Add example API documentation (example requests/responses) and a sample appsettings.Development.json template without secrets.
- Add automated tests for scheduling logic and CI build pipeline.
