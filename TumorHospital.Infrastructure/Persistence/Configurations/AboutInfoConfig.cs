using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class AboutInfoConfig : IEntityTypeConfiguration<AboutInfo>
    {
        public void Configure(EntityTypeBuilder<AboutInfo> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.HospitalName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(a => a.Email)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(a => a.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(a => a.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
        }
    }
}