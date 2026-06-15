
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

# 🔐 Security

- JWT Authentication
- Role-Based Access Control:
  - Admin
  - Doctor
  - Receptionist
  - Patient

---

# 🚀 Key Design Patterns

- Repository Pattern
- Unit of Work
- CQRS (light usage)
- Specification Pattern
- Strategy pattern (external services)
- Options Pattern
- Dependency Injection

---

# 🧠 Final Notes

This system is built to reflect real hospital-grade constraints:
- Time-sensitive operations
- Concurrency-safe appointment handling
- Strong domain validation
- External integrations (AI + Payments)

The most critical engineering challenge solved here is:

> **Preventing race conditions in appointment approval using optimistic concurrency control**

---

# 📌 End of Documentation
