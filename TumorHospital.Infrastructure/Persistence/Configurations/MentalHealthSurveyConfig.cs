using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class MentalHealthSurveyConfig : IEntityTypeConfiguration<MentalHealthSurvey>
    {
        public void Configure(EntityTypeBuilder<MentalHealthSurvey> builder)
        {
            builder.HasKey(mhs => mhs.Id);

            builder.Property(mhs => mhs.AnxietyScore)
                .IsRequired();

            builder.Property(mhs => mhs.DepressionScore)
                .IsRequired();

            builder.Property(mhs => mhs.StressScore)
                .IsRequired();

            builder.Property(mhs => mhs.SurveyDate)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(mhs => mhs.Patient).WithMany(p => p.MentalHealthSurvies)
                .HasForeignKey(mhs => mhs.PatientId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
