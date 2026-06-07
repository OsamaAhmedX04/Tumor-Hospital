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

            builder.Property(d => d.PredictedClass)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.ImageURL)
                .IsRequired();

            builder.Property(d => d.ConfidenceScore)
                .HasColumnType("DECIMAL(5,3)")
                .IsRequired();

            builder.Property(d => d.GliomaProbability)
                .HasColumnType("DECIMAL(5,3)")
                .IsRequired();

            builder.Property(d => d.MeningiomaProbability)
                .HasColumnType("DECIMAL(5,3)")
                .IsRequired();

            builder.Property(d => d.NoTumorProbability)
                .HasColumnType("DECIMAL(5,3)")
                .IsRequired();

            builder.Property(d => d.PituitaryProbability)
                .HasColumnType("DECIMAL(5,3)")
                .IsRequired();

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasOne(d => d.Appointment)
                .WithOne(a => a.Diagnostic)
                .HasForeignKey<Diagnostic>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
