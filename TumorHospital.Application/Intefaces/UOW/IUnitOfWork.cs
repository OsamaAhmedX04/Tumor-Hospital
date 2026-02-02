using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Intefaces.UOW
{
    public interface IUnitOfWork
    {
        IRepository<RefreshTokenAuth> RefreshTokenAuths { get; }

        IRepository<Admin> Admins { get; }
        IRepository<Doctor> Doctors { get; }
        IRepository<Patient> Patients { get; }
        IRepository<Receptionist> Receptionists { get; }

        IRepository<Hospital> Hospitals { get; }
        IRepository<Bill> Bills { get; }
        IRepository<Offer> Offers { get; }
        IRepository<CharityNeed> CharityNeeds { get; }
        IRepository<VolunteerDonation> VolunteerDonations { get; }

        IRepository<Appointment> Appointments { get; }
        IRepository<DoctorSchedule> DoctorSchedules { get; }
        IRepository<Specialization> Specializations { get; }

        IRepository<MedicalRecord> MedicalRecords { get; }
        IRepository<Diagnostic> Diagnostics { get; }
        IRepository<MentalHealthSurvey> MentalHealthSurvies { get; }
        IRepository<Prescription> Prescriptions { get; }

        IRepository<Notification> Notifications { get; }
        IRepository<FAQ> FAQs { get; }

        IRepository<AboutInfo> AboutInfos { get; }

        Task<int> CompleteAsync();
    }
}
