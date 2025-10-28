using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class DiagnosticConfig : IEntityTypeConfiguration<Diagnostic>
    {
        public void Configure(EntityTypeBuilder<Diagnostic> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.ConfidenceScore)
                .HasColumnType("DECIMAL(5,2)")
                .IsRequired();

            builder.Property(d => d.ModelOutput)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.TumorLocation)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(d => d.MedicalRecord).WithMany(md => md.Diagnostics)
                .HasForeignKey(d => d.MedicalRecordId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
