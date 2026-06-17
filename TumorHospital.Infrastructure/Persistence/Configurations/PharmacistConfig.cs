using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class PharmacistConfig : IEntityTypeConfiguration<Pharmacist>
    {
        public void Configure(EntityTypeBuilder<Pharmacist> builder)
        {
            builder.HasKey(p => p.ApplicationUserId);

            builder.Property(x => x.HireDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User)
                .WithMany(p => p.Pharmacists)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Pharmacy)
                .WithMany(p => p.pharmacists)
                .HasForeignKey(x => x.PharmacyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
