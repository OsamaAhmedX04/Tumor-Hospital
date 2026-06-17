using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class SupplierConfig : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ContactPersonName)
                .HasMaxLength(200);

            builder.Property(x => x.ContactPersonPhone)
                .HasMaxLength(30);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
