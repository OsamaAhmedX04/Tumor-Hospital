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
    public class ReceptionistConfig : IEntityTypeConfiguration<Receptionist>
    {
        public void Configure(EntityTypeBuilder<Receptionist> builder)
        {
            builder.HasKey(p => p.ApplicationUserId);

            builder.Property(d => d.Gender).HasMaxLength(10).IsRequired();
            builder.Property(d => d.Address).HasMaxLength(200).IsRequired();
            builder.Property(d => d.RegistrationDate).HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(d => d.User).WithMany(u => u.Receptionists)
                .HasForeignKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
