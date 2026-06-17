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
    public class MedicinePurchaseOrderConfig : IEntityTypeConfiguration<MedicinePurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<MedicinePurchaseOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2).IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Status)
                .HasConversion<string>().IsRequired();

            builder.HasOne(x => x.Medicine)
                .WithMany(m => m.purchaseOrders)
                .HasForeignKey(x => x.MedicineId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Pharmacy)
                .WithMany(m => m.purchaseOrders)
                .HasForeignKey(x => x.PharmacyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Supplier)
                .WithMany(m => m.purchaseOrders)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.CreatedByPharmacist)
                .WithMany(m => m.purchaseOrders)
                .HasForeignKey(x => x.CreatedByPharmacistId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
