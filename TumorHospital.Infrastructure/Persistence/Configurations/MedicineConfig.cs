using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class MedicineConfig : IEntityTypeConfiguration<Medicine>
    {
        public void Configure(EntityTypeBuilder<Medicine> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.QuantityInStock)
                .IsRequired();

            builder.Property(x => x.MinimumQuantity)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.CreatedByPharmacist)
                .WithMany(p => p.MedicinesCreated)
                .HasForeignKey(x => x.CreatedByPharmacistId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Supplier)
                .WithMany(p => p.Medicines)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
