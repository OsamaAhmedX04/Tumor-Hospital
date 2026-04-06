using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(p => p.ApplicationUserId);

            builder.Property(d => d.Gender).HasMaxLength(10).IsRequired();
            builder.Property(d => d.RegistrationDate).HasDefaultValueSql("GETDATE()");
            builder.Property(d => d.IsVideoCallDoctor).HasDefaultValue(false);

            builder.Property(d => d.ConsultationCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(d => d.FollowUpCost)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(d => d.VideoCallCost)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(d => d.User).WithMany(u => u.Doctors)
                .HasForeignKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(d => d.Specialization).WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
