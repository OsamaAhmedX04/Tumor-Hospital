using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class CharityNeedConfig : IEntityTypeConfiguration<CharityNeed>
    {
        public void Configure(EntityTypeBuilder<CharityNeed> builder)
        {
            builder.HasKey(cn => cn.Id);

            builder.Property(cn => cn.Title)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(cn => cn.Description)
                .IsRequired()
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(cn => cn.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(cn => cn.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(cn => cn.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(cn => cn.NeedAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cn => cn.CollectedAmount)
                .HasDefaultValue(0)
                .HasColumnType("decimal(18,2)");

        }
    }
}
