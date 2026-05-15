using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.Persistence.Context;
using TumorHospital.Infrastructure.Persistence.Repositories;

namespace TumorHospital.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IRepository<RefreshTokenAuth> RefreshTokenAuths { get; }

        public IRepository<Admin> Admins { get; }

        public IRepository<Doctor> Doctors { get; }

        public IRepository<Patient> Patients { get; }

        public IRepository<Receptionist> Receptionists { get; }


        public IRepository<Hospital> Hospitals { get; }

        public IRepository<Bill> Bills { get; }
        public IRepository<Offer> Offers { get; }

        public IRepository<CharityNeed> CharityNeeds { get; }

        public IRepository<VolunteerDonation> VolunteerDonations { get; }


        public IRepository<Appointment> Appointments { get; }

        public IRepository<DoctorSchedule> DoctorSchedules { get; }

        public IRepository<Specialization> Specializations { get; }


        public IRepository<MedicalRecord> MedicalRecords { get; }

        public IRepository<Diagnostic> Diagnostics { get; }

        public IRepository<MentalHealthSurvey> MentalHealthSurvies { get; }

        public IRepository<Prescription> Prescriptions { get; }


        public IRepository<Notification> Notifications { get; }

        public IRepository<FAQ> FAQs { get; }

        public IRepository<AboutInfo> AboutInfos { get; }

        public IRepository<VideoCall> VideoCalls { get; }


        public UnitOfWork(AppDbContext db)
        {
            _db = db;

            RefreshTokenAuths = new Repository<RefreshTokenAuth>(_db);

            Admins = new Repository<Admin>(_db);
            Patients = new Repository<Patient>(_db);
            Doctors = new Repository<Doctor>(_db);
            Receptionists = new Repository<Receptionist>(_db);

            Hospitals = new Repository<Hospital>(_db);
            Bills = new Repository<Bill>(_db);
            Offers = new Repository<Offer>(_db);
            CharityNeeds = new Repository<CharityNeed>(_db);
            VolunteerDonations = new Repository<VolunteerDonation>(_db);

            Appointments = new Repository<Appointment>(_db);
            DoctorSchedules = new Repository<DoctorSchedule>(_db);
            Specializations = new Repository<Specialization>(_db);

            MedicalRecords = new Repository<MedicalRecord>(_db);
            MentalHealthSurvies = new Repository<MentalHealthSurvey>(_db);
            Diagnostics = new Repository<Diagnostic>(_db);
            Prescriptions = new Repository<Prescription>(_db);

            Notifications = new Repository<Notification>(_db);
            FAQs = new Repository<FAQ>(_db);
            AboutInfos = new Repository<AboutInfo>(_db);
            VideoCalls = new Repository<VideoCall>(_db);

        }

        public async Task<int> CompleteAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();


    }
}
