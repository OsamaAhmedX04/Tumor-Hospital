
# 🏥 Tumor Hospital Management System

A production-grade healthcare management platform built with ASP.NET Core Web API following Clean Architecture principles.

It orchestrates hospital operations including appointments, prescriptions, donations, AI diagnostics, scheduling, payments, and role-based access control.

---

# 📌 System Overview

This system is designed for oncology hospital workflows:

- Patient appointment booking & lifecycle management
- Doctor scheduling & availability
- Prescription creation after appointment completion
- Charity donation system with payment integration
- AI-powered MRI diagnostic analysis
- Hospital administration (doctors, receptionists, departments)
- Offer & promotion system with background activation
- Secure authentication & role-based authorization

---

# 🏗️ Architecture

## Clean Architecture Layers

### Domain Layer
- Core business entities:
  Appointment, Doctor, Patient, Prescription, Donation, Hospital, Schedule, Offer
- Enums:
  AppointmentStatus, DayOfWeek, CharityCategory

### Application Layer
- DTOs (Request/Response models)
- Service interfaces
- Business rules abstraction

### Infrastructure Layer
- EF Core repositories
- Unit of Work implementation
- External integrations:
  - Payment Gateway
  - AI ML Service
  - File Storage (Supabase)
  - Hangfire background jobs

### API Layer
- Controllers
- Middleware (Error handling, Auth, Rate limiting)
- JWT Authentication

---

# 🏥 Authentication Services – Business Overview

This document describes the **business logic** behind the Tumor Hospital authentication system. It focuses on **what each service does for the hospital**, not technical endpoints.

---

## 👑 AdminService – Staff & System Management

**Purpose:** Enables hospital administrators to manage doctors, receptionists, and oversee system activity.

### Business Capabilities

| Business Need | How AdminService Solves It |
|---------------|----------------------------|
| **Hire a new doctor** | Admin creates a doctor account → system generates a random password and emails it to the doctor. The doctor is placed in an "inactive" state until they set their own password. |
| **Hire a new receptionist** | Same flow as doctor: admin creates account, random password emailed, role set to inactive receptionist. |
| **Remove a staff member** | Admin soft‑deletes a doctor or receptionist. The account remains in the database (for legal records) but cannot log in. The staff member is also unassigned from their hospital. |
| **Reactivate a staff member** | Admin can restore a soft‑deleted account, allowing the person to log in again. |
| **View hospital dashboard** | Admin sees key metrics: total patients, doctors, receptionists, appointments, bills, revenue, pending/cancelled bills, and charity needs. |
| **Ensure hospital capacity** | When creating a doctor/receptionist, the system checks if the hospital has not exceeded its max allowed staff. Prevents over‑hiring. |

### Business Flow – Hiring a Doctor

1. Hospital admin enters doctor details (name, email, specialization, hospital).
2. System validates that the hospital has available slots.
3. System creates a user account with a secure random password.
4. The doctor receives an email with their temporary password.
5. The doctor **must** log in and change the password before accessing any medical features.
6. The doctor’s role becomes active (`Doctor` instead of `InActiveDoctor`).

> ✅ This ensures every staff member chooses their own password while allowing centralised onboarding.

---

## 🔐 AuthService – Patient & Staff Authentication

**Purpose:** Manages all login, registration, password, and token operations for patients and staff.

### Patient Journey (Self‑Service)

| Step | Business Logic |
|------|----------------|
| **Register** | Patient provides name, email, password, gender. System creates an account with `EmailConfirmed = false`. A 6‑digit confirmation token is sent by email. |
| **Confirm email** | Patient enters the 6‑digit token. System activates the account, issues a JWT (access token) and a refresh token for future logins. |
| **Login** | Patient provides email and password. System checks email confirmation, account not deleted, and credentials. On success, returns access + refresh token. |
| **Logout** | System invalidates the refresh token so the session cannot be extended. |
| **Change password** | Logged‑in patient provides old and new password. System validates and updates. |
| **Forgot password** | Patient requests a reset → system emails a 6‑digit token. Patient uses token + new password to reset. |

### Staff Account Activation (First‑time login)

**Critical for new doctors/receptionists created by admin:**

1. Staff receives an email with a random password.
2. Staff logs in using that password.
3. System detects the account is in `InActiveDoctor` or `InActiveReceptionist` role.
4. Staff is **forced** to change their password (old password = the random one, new password = their choice).
5. After successful change, the role is upgraded to active (`Doctor` or `Receptionist`).
6. Staff now has full access to their assigned duties.

> 🔁 This flow ensures every staff member uses a personal, secret password while allowing admin to onboard quickly.

### Refresh Token – Seamless Session Extension

- When access token expires, the user (patient or staff) can obtain a new access token using the refresh token.
- The old refresh token is **rotated** (replaced with a new one) to prevent reuse attacks.
- Refresh token expiry depends on whether the user checked "Remember me" (30 days) or not (1 day).

### Password Reset for All Users

- **Forgot password** triggers a 6‑digit token sent to the registered email.
- **Resend token** allows requesting a new token if the first one expired (1‑hour validity).
- **Reset password** validates the token and updates the password.

---

## 🛡️ Role‑Based Access Control (Business Rules)

The system enforces the following **business rules**:

| Role | What they can do in the hospital |
|------|----------------------------------|
| **Admin** | Create/delete staff, view all dashboards, manage hospitals, specializations, offers, charity needs. |
| **Doctor** | Manage own schedule, accept/reject appointments, write prescriptions, view patient diagnostics, conduct video calls. |
| **Receptionist** | Receive cash payments, mark patient attendance, view appointment lists. |
| **Patient** | Book appointments (consultation, follow‑up, video call), view own bills, upload MRI scans, get AI diagnostic reports, donate to charity. |
| **InActiveDoctor / InActiveReceptionist** | **Only** allowed to change password (first login). Cannot access any other system features until activated. |

---

## 📧 Email Notifications – Business Triggers

The system automatically sends emails for these business events:

- **Patient registration** → confirmation token.
- **Patient confirms email** → (no email, but token consumed).
- **Resend confirmation** → new token email.
- **Forgot password** → reset token email.
- **Resend reset token** → new reset token email.
- **Admin creates doctor** → email with random password and activation instructions.
- **Admin creates receptionist** → same as doctor.

All emails are sent via **SendGrid** and contain the hospital’s branding.

---

## 🚦 Security & Compliance – Business Safeguards

- **Rate limiting** prevents brute‑force attacks on registration, login, and password reset (100 attempts per minute per IP).
- **Soft delete** keeps patient/staff records for legal retention but blocks access.
- **Refresh token rotation** protects against session hijacking.
- **Email confirmation** ensures real patient ownership of the email address.
- **First‑time password change** guarantees that only the intended staff knows the final password.

---

## 🔄 Business Process Summary

### Onboarding a new patient
1. Patient fills registration form.
2. System sends confirmation code.
3. Patient confirms email.
4. Patient can log in and book appointments.

### Hiring a new doctor (admin)
1. Admin fills doctor details.
2. System emails random password.
3. Doctor logs in with that password.
4. Doctor is forced to change password.
5. Doctor can now manage schedule and appointments.

### Handling a forgotten password (any user)
1. User requests "forgot password".
2. System emails a 6‑digit code.
3. User enters code + new password.
4. Password is updated.

---

## 📊 Key Metrics Available to Admin

The admin dashboard shows:
- Total patients, doctors, receptionists, appointments, bills, charity needs.
- Total revenue (sum of paid bills).
- Pending and cancelled bills count.
- Completed charity needs.

This gives hospital management real‑time oversight.

---

## 📝 Notes for Hospital Management

- **Doctors/receptionists** cannot be fully deleted – only soft‑deleted to preserve audit trails.
- **Password policies** enforce strong passwords (minimum length, complexity) to reduce security risks.
- **Session timeout** is controlled by JWT expiry (default 15 Minutes) – users are automatically logged out after that unless they use refresh token.
- **Remember me** extends refresh token lifetime to 30 days for convenience.

---





# 📅 Appointment System (Core Heart of the System)

The appointment system is the most critical and concurrency-sensitive module.

It handles:
- Booking appointment slots
- Approval/rejection by receptionists or doctors
- Scheduling constraints
- Conflict prevention
- Status transitions

---

# 🔄 Appointment Lifecycle

An appointment follows this lifecycle:

## 1. Created (Pending)
- Patient requests appointment
- Status = `Pending`
- Slot is not yet confirmed

## 2. Approved or Rejected
- Receptionist/Doctor reviews request
- Status transitions:
  - Pending → Approved
  - Pending → Rejected

## 3. Scheduled Execution Window
- Appointment time arrives
- Doctor attends patient

## 4. Completed
- Consultation finished
- Prescription can be created AFTER this stage

## 5. Archived State
- Historical record kept for reporting

---

# ⚠️ Complexity of Appointment System

This system is NOT just CRUD.

It involves:

## 1. Time-slot conflicts
- Prevent overlapping appointments
- Ensure doctor availability

## 2. Status integrity
- Prevent invalid transitions (e.g., Rejected → Approved)

## 3. Multi-role access
- Patient creates
- Receptionist approves
- Doctor executes

## 4. Real-time concurrency risk
- Multiple staff members may act on the same appointment simultaneously

---

# 💥 Race Condition Problem (Accept/Reject)

## Scenario

Two receptionists open the same appointment:

- User A clicks "Approve"
- User B clicks "Reject" at the same time

Both requests reach the server nearly simultaneously.

Without protection:
- Both updates execute
- Final state depends on last write
- System becomes inconsistent

This is a classic **lost update problem**.

---

# 🧠 Why This Happens

Because:
- EF Core tracks entities independently per request
- Database update is last-write-wins by default
- No synchronization between concurrent requests

---

# 🛡️ Solution: Optimistic Concurrency Control

## What is it?

Optimistic concurrency assumes:
> Conflicts are rare, but must be detected before commit.

Instead of locking rows, we detect changes.

---

## Implementation Strategy

### 1. Add Concurrency Token

In Appointment entity:

```csharp
public class Appointment
{
    public Guid Id { get; set; }

    public AppointmentStatus Status { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
```

---

### 2. EF Core Behavior

EF Core uses `RowVersion`:

- Reads current version from DB
- Sends it back during update
- Compares before saving

---

### 3. Conflict Detection

When two updates happen:

- Request A reads RowVersion = 101
- Request B reads RowVersion = 101

Then:
- A updates → RowVersion becomes 102
- B tries update with old version (101)

EF Core throws:

```
DbUpdateConcurrencyException
```

---

### 4. Handling Conflict

We catch it:

```csharp
try
{
    await _unitOfWork.CompleteAsync();
}
catch (DbUpdateConcurrencyException)
{
    throw new Exception("This appointment was modified by another user. Please refresh.");
}
```

---

# 🧩 Result

- No lost updates
- No inconsistent appointment state
- Clear conflict feedback to user
- System remains scalable (no locking overhead)

---

# 💊 Prescription Rule Enforcement

Prescriptions can ONLY be created when:

- Appointment time has passed
- Appointment is completed/approved

Logic prevents premature creation:

- Prevents medical misuse
- Ensures chronological integrity

---

# 💰 Donation System Complexity

Includes:
- Payment gateway integration
- Invoice generation
- Webhook validation
- Donation state tracking

Flow:
1. Create donation (Pending)
2. Generate invoice
3. Redirect to payment provider
4. Handle webhook
5. Mark Paid/Failed
6. Update charity need progress

---

# 🏥 Hospital Management Module

Handles:
- Hospital creation
- Doctor assignment
- Receptionist allocation
- Capacity constraints:
  - Max doctors
  - Max receptionists

Includes validation:
- Prevent over-allocation
- Prevent deletion if staff exists

---

# 📆 Scheduling System

Doctors have weekly schedules:
- Max 5 working days
- Fixed shift duration (8 hours)
- 30-minute appointment slots

Features:
- Availability checking
- Slot generation
- Conflict detection with appointments

---

# 🤖 AI Diagnostic Module

Workflow:
1. Upload MRI scan
2. Send to ML API
3. Receive prediction:
   - Glioma
   - Meningioma
   - Pituitary tumor
   - No tumor
4. Store diagnostic result
5. Link to appointment

---

# ⚙️ Background Jobs (Hangfire)

Used for:
- Offer activation/deactivation
- Scheduled tasks
- Delayed operations

Example:
- Offer starts in future → scheduled activation job
- Offer ends → automatic deactivation

---

# 🔒 Security Controls

## 🔁 Dual-Token Authentication

- Short‑lived JWT access token – expires in **15 minutes** – reduces window for token misuse.
- Refresh token stored in **HttpOnly Secure cookie** (not accessible via JavaScript) – protects against XSS token theft.
- Refresh token lifetime: **1 day** (normal) or **30 days** (with "Remember Me").
- Automatic **rotation** – each refresh invalidates the old refresh token and issues a new one, preventing replay attacks.

**Business impact:** Minimizes risk of session hijacking; users stay logged in safely without re‑entering credentials frequently.

---

## ✅ Input Validation (FluentValidation)

- All API request DTOs validated **before** entering business logic.
- Field‑level rules: email format, password complexity, required fields, string lengths, enums.
- Invalid requests immediately return **400 Bad Request** with detailed field errors (no processing).
- Prevents malformed data, injection attempts, and business rule violations at the gate.

**Business impact:** Blocks malicious or corrupted data early; reduces debugging time and protects database integrity.

---

## 🛡️ SQL Injection Prevention

- Entity Framework Core used exclusively with **parameterized LINQ queries**.
- No raw SQL string concatenation or interpolation anywhere in the codebase.
- All user input passed as parameters – impossible for attackers to alter query structure.
- Automated checks in CI/CD to detect accidental raw SQL.

**Business impact:** Eliminates the most dangerous database attack vector – patient records remain confidential and uncorrupted.

---

## 🌐 Strict CORS Policy

- Explicit **allowlist** of trusted origins (frontend domains, hospital portals).
- Wildcard origins (`*`) **prohibited** in production – no unrestricted cross‑origin calls.
- Only configured HTTP methods and headers are permitted.
- Preflight requests (OPTIONS) handled correctly for complex requests.

**Business impact:** Prevents unauthorized websites from calling the API; stops cross‑site request forgery (CSRF) and data leaks.

---

## 🚦 Rate Limiting (DoS Prevention)

- Returns `429 Too Many Requests` with retry‑after header.

**Business impact:** Mitigates brute‑force password attacks, credential stuffing, and denial‑of‑service attempts – system remains responsive.

---

## 📜 Audit Logging

- Dedicated **audit log file** (structured JSON via Serilog) recording every critical event.
- Logged events: login attempts (success/failure), token refresh operations, role changes (e.g., InActive → Doctor), payments (bills, donations), file uploads (MRI scans, profile pictures).
- Each log includes: timestamp, user ID (if authenticated), IP address, action type, outcome.
- Audit logs stored separately from application logs – append‑only, tamper‑evident.

**Business impact:** Provides full accountability for forensic investigations, compliance audits, and breach analysis.

---


# 🚀 Key Design Patterns

- Repository Pattern
- Unit of Work
- Specification Pattern
- Options Pattern
- Dependency Injection

---

# 🧠 Final Notes

This system is built to reflect real hospital-grade constraints:
- Time-sensitive operations
- Concurrency-safe appointment handling
- Strong domain validation
- External integrations (AI + Payments)

---

# 📌 End of Documentation
