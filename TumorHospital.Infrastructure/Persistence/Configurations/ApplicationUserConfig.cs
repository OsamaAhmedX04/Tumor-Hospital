using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(au => au.FirstName).HasMaxLength(40).IsRequired();
            builder.Property(au => au.LastName).HasMaxLength(40).IsRequired();
            builder.Property(au => au.IsActive).HasDefaultValue(false);
        }
    }
}
