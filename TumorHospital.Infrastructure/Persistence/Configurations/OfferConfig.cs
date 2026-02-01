using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Description)
                .HasMaxLength(500);

            builder.Property(o => o.DiscountPercentage)
                .IsRequired()
                .HasColumnType("DECIMAL(5,2)");

            builder.Property(o => o.StartDate)
                .IsRequired();

            builder.Property(o => o.EndDate)
                .IsRequired();

            builder.Property(o => o.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
