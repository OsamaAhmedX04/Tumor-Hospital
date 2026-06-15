# 🏥 Tumor Hospital Management System

A full-featured hospital management platform built with ASP.NET Core Web API, designed to manage hospital operations, appointments, donations, AI diagnostics, scheduling, and more. The system follows Clean Architecture and integrates multiple external services.

---

# 📌 Overview

The system manages:
- Patients, Doctors, Receptionists, Admins
- Appointments & scheduling engine
- MRI AI diagnostics
- Donations & charity system
- Offers & hospital management

---

# 🏗️ Architecture

Clean Architecture:

Presentation → Application → Domain → Infrastructure

Patterns:
- Repository Pattern
- Unit of Work
- Dependency Injection
- Options Pattern

---

# 📅 Appointment System

Core system responsible for booking, scheduling, and managing hospital visits.

## Lifecycle

1. Pending → created by patient
2. Approved → confirmed by reception/admin
3. Rejected → canceled by system/admin
4. Completed → visit finished

---

# ⚠️ Complexity

Appointment system handles:
- Doctor availability constraints
- Time slot validation
- Overlapping prevention
- Payment integration
- External AI workflows

---

# 💥 Race Condition Problem

Two users may try to approve/reject the same appointment simultaneously leading to:
- Double approval
- Data inconsistency

---

# 🧷 Solution: Optimistic Concurrency

Using EF Core RowVersion:

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

# ⚡ How it works

EF Core ensures:

UPDATE ... WHERE Id = X AND RowVersion = OLD

If mismatch → DbUpdateConcurrencyException

---

# 🛡️ Handling Conflict

```csharp
try
{
    await _unitOfWork.CompleteAsync();
}
catch (DbUpdateConcurrencyException)
{
    throw new Exception("Appointment was modified by another user");
}
```

---

# 🔄 Scheduling Engine

- 30-minute slots
- Doctor weekly schedules
- No overlap rules
- Max 5 working days per doctor

---

# 💳 Integrations

- Payment Gateway (Fawaterak)
- AI MRI classification
- Supabase storage
- SendGrid emails
- Hangfire background jobs

---

# 🔐 Security

- JWT Authentication
- Role-Based Access Control
- Webhook validation
- Input validation

---

# 🚀 Summary

This system is a hospital workflow engine that ensures:
- Consistency
- Concurrency safety
- Scalable scheduling
- External service integration
