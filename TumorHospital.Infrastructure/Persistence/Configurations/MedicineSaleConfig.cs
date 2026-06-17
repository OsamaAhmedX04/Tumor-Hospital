using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class MedicineSaleConfig : IEntityTypeConfiguration<MedicineSale>
    {
        public void Configure(EntityTypeBuilder<MedicineSale> builder)
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

            builder.HasOne(x => x.Medicine)
                .WithMany(m => m.MedicineSales)
                .HasForeignKey(x => x.MedicineId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Pharmacy)
                .WithMany(m => m.MedicineSales)
                .HasForeignKey(x => x.PharmacyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.SoldByPharmacist)
                .WithMany(m => m.MedicineSales)
                .HasForeignKey(x => x.SoldByPharmacistId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
