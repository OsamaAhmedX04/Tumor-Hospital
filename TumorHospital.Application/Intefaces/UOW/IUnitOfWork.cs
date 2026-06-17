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
        IRepository<Diagnostic> Diagnostics { get; }
        IRepository<Prescription> Prescriptions { get; }

        IRepository<FAQ> FAQs { get; }

        IRepository<AboutInfo> AboutInfos { get; }

        IRepository<VideoCall> VideoCalls { get; }

        IRepository<Pharmacy> Pharmacies { get; set; }
        IRepository<Pharmacist> Pharmacists { get; set; }
        IRepository<Medicine> Medicines { get; set; }
        IRepository<Supplier> Suppliers { get; set; }
        IRepository<MedicinePurchaseOrder> MedicinePurchaseOrders { get; set; }
        IRepository<MedicineSale> MedicineSales { get; set; }


        Task<int> CompleteAsync();
    }
}
