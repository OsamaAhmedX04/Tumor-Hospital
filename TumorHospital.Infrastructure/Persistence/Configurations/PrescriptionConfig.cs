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
    public class PrescriptionConfig : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();


            builder
                .HasOne(p => p.Patient).WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId).OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(p => p.Doctor).WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
