using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasData(new IdentityUserRole<string>
            {
                UserId = "b999c3a1-4e99-4d8a-9f99-2c997b1e3d99", // Admin user ID
                RoleId = "9f8d4b9b-1e52-4dbf-a356-2c62b9db5b01"  // Admin role ID
            });
        }
    }
}
