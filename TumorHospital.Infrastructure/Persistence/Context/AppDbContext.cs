using Microsoft.EntityFrameworkCore;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.Persistence.Configurations;

namespace TumorHospital.Infrastructure.Persistence.Context
{
    public class AppDbContext : CustomIdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // its already exist in IdentityDbContext base class
        //public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<RefreshTokenAuth> RefreshTokenAuths { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<CharityNeed> CharityNeeds { get; set; }
        public DbSet<VolunteerDonation> VolunteerDonations { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MentalHealthSurvey> MentalHealthSurvies { get; set; }
        
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(RoleConfig).Assembly);
        }
    }
}
