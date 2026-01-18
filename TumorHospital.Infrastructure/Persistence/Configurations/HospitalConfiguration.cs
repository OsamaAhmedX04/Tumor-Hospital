using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.HasKey(h => h.Id);


            builder.Property(h => h.Government)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.Address)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(h => h.MaxNumberOfDoctors)
                .IsRequired();

            builder.Property(h => h.MaxNumberOfReceptionists)
                .IsRequired();

            builder.HasMany(h => h.Doctors)
                .WithOne(d => d.Hospital)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(h => h.Receptionists)
                .WithOne(r => r.Hospital)
                .HasForeignKey(r => r.HospitalId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
