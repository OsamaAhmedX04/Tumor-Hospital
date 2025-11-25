using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class BillConfig : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(b => b.TotalAmount)
                .IsRequired()
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(b => b.Code)
                .IsRequired();

            builder.HasIndex(b => b.Code).IsUnique();

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            builder
                .HasOne(b => b.Patient).WithMany(p => p.Bills)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Receptionist).WithMany(r => r.Bills)
                .HasForeignKey(b => b.ConfirmedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
