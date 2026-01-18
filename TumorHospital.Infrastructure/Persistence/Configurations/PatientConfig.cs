using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.ApplicationUserId);

            builder.Property(p => p.Gender).HasMaxLength(10).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(200);
            builder.Property(p => p.RegistrationDate).HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(p => p.User).WithMany(u => u.Patients)
                .HasForeignKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
