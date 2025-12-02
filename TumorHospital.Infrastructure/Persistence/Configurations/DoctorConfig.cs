using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
