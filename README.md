# Intelligent Tumor Hospital Backend (ASP.NET Core .NET 8)

## 📌 Project Overview
This backend is part of a healthcare management platform designed to support hospital operations, focusing on:
- Patient management  
- Doctor portal  
- Appointment scheduling  
- AI-powered diagnostic integration  
- Medical history & document handling  
- Mental health chatbot integration (API-ready)  
- Prescription management  
- Notifications & billing (future-ready)

This README covers only the **Backend (.NET 8)** implementation using **Clean Architecture**, **SQL Server**, and **Identity + JWT Authentication**.

---

## 🛠️ Technologies Used
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity (with JWT Authentication)
- Clean Architecture (Domain, Application, Infrastructure, API)
- Swagger / Swashbuckle
- Dependency Injection
- AutoMapper (if applicable)
- FluentValidation (if used)

---

## 🏗️ Architecture Overview
This backend follows a layered Clean Architecture structure:

```
├── RentMate.Domain            → Entities, Value Objects, Interfaces
├── RentMate.Application       → Business Logic, Services, DTOs, Validation
├── RentMate.Infrastructure    → EF Core, Identity, Migrations, Repositories
└── RentMate.API               → Controllers, DI, Startup, Swagger
```

---

## ✅ Features Implemented (Backend Only)
✔ Patient Registration & Profiles  
✔ Doctor Registration & Profiles  
✔ Identity + JWT Authentication  
✔ Role-Based Authorization (Patient, Doctor, Admin)  
✔ Medical Records & Upload Support  
✔ Appointment Scheduling API  
✔ Diagnostic Model Integration Ready (REST-based)  
✔ Treatment & Prescription Endpoints  
✔ Admin Operations  
✔ Secure Data Handling (Encryption + EF Tracking)

### Upcoming / Planned:
⬜ Billing & Insurance APIs  
⬜ Mental Health Chatbot Integration  
⬜ Analytics & Dashboard Reports  
⬜ Notification Services

---

## 📦 Prerequisites
Make sure you have:
- .NET 8 SDK installed  
- SQL Server (LocalDB or full instance)  
- Visual Studio / Rider / VS Code  
- Postman / Swagger for testing  

---

## ⚙️ Configuration & Setup

### 1️⃣ Clone the Repo
```bash
git clone <your-repo-url>
cd <your-backend-folder>
```

### 2️⃣ Configure `appsettings.json` in **API** project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HospitalDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "DurationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 🗄️ Database Setup
### Run EF Core Migrations
```bash
dotnet ef database update
```

If there are no migrations yet:
```bash
dotnet ef migrations add InitialCreate -p RentMate.Infrastructure -s RentMate.API
```

---

## ▶️ Run the Application
From the API project directory:
```bash
dotnet run
```

Or using Visual Studio (F5).

Swagger UI will be available at:  
🔗 `https://localhost:<port>/swagger`

---

## 🔐 Authentication Flow (Identity + JWT)
- User registers (Patient / Doctor / Admin)  
- API generates JWT after login  
- Token is passed via `Authorization: Bearer <token>`  
- Role-based authorization applied on controllers  

---

## 🌍 Environment Variables (Optional)
```
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection="..."
Jwt__Key="..."
Jwt__Issuer="..."
Jwt__Audience="..."
```

---

## 📚 API Overview (Examples)

### Auth Endpoints
```
POST /api/auth/register
POST /api/auth/login
```

### Patients
```
GET /api/patients
POST /api/patients
```

### Doctors
```
GET /api/doctors
POST /api/doctors
```

### Appointments
```
POST /api/appointments
GET /api/appointments/{id}
```

(More endpoints can be detailed in Swagger or Postman)

---

## ✅ Contribution Guidelines
- Follow Clean Architecture best practices  
- Use feature-based folders where suitable  
- Follow naming conventions for DTOs, Services, and Controllers  
- Ensure migrations remain consistent  

---

## 🚀 Future Enhancements
- Payment & Billing Module  
- Notification Services (Email/SMS/Twilio)  
- Chatbot Integration APIs  
- Analytics & Dashboard KPIs  
- Cloud Deployment Support  

---

## 📝 License
Internal / Academic Use Only (Update as needed)

---

## ✅ Notes
- Team member names intentionally excluded as requested  
- Adjust database connection and JWT keys before production  
