using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class MedicalRecordConfig : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(md => md.Id);

            builder.Property(md => md.RecordType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(md => md.FilePath).IsRequired();

            builder.Property(md => md.UploadDate)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(md => md.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(p => p.PatientId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
