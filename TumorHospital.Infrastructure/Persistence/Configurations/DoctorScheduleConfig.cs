using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class DoctorScheduleConfig : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => ds.Id);

            builder.Property(ds => ds.DayOfWeek)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Property(ds => ds.StartTime)
                .IsRequired()
                .HasColumnType("TIME");

            builder.Property(ds => ds.EndTime)
                .IsRequired()
                .HasColumnType("TIME");

            builder.Property(ds => ds.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(ds => ds.IsAvailable).HasDefaultValue(true);


            builder
                .HasOne(ds => ds.Doctor).WithMany(d => d.Schedules)
                .HasForeignKey(ds => ds.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
