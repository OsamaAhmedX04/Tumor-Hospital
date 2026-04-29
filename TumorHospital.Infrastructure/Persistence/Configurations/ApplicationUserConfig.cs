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

            var admin = new ApplicationUser
            {
                Id = "b999c3a1-4e99-4d8a-9f99-2c997b1e3d99",
                UserName = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "Hospital",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                IsActive = true,
                SecurityStamp = "b7f9c3a1-4e2d-4d8a-9f5e-2c6a7b1e3d90",
                ConcurrencyStamp = "d1111111-1111-1111-1111-111111111111",
                PasswordHash = "AQAAAAIAAYagAAAAEIxf/T4+JM9mGdIzJeillyHuMI4W/4VWohrIBR5adtn7GnJcQUkDWSkwgtpNiKivgw=="
            };

            builder.HasData(admin);
        }
    }
}
