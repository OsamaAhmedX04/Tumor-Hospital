using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("Roles");

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "9f8d4b9b-1e52-4dbf-a356-2c62b9db5b01",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                new IdentityRole
                {
                    Id = "a7e13e2a-63a7-4c5d-a5b9-5b49efb0123f",
                    Name = "Doctor",
                    NormalizedName = "DOCTOR",
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                },
                new IdentityRole
                {
                    Id = "c3e4c1a9-4f7e-4a42-9c36-8d2a6e9c75bd",
                    Name = "InActiveDoctorRole",
                    NormalizedName = "INACTIVEDOCTORROLE",
                    ConcurrencyStamp = "22222222-2222-2200-0000-000000000000"
                },
                new IdentityRole
                {
                    Id = "3f209613-7a40-4a9b-b270-ff3a39a1337c",
                    Name = "InActiveReceptionistRole",
                    NormalizedName = "INACTIVERECEPTIONISTROLE",
                    ConcurrencyStamp = "33333333-3333-3300-0000-000000000000"
                },
                new IdentityRole
                {
                    Id = "b3f14a23-34a5-4bc0-912d-0f2f1d8d4a11",
                    Name = "Receptionist",
                    NormalizedName = "RECEPTIONIST",
                    ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
                },
                new IdentityRole
                {
                    Id = "c4f72d94-5632-4cb1-9c33-2a72e1c77788",
                    Name = "Patient",
                    NormalizedName = "PATIENT",
                    ConcurrencyStamp = "44444444-4444-4444-4444-444444444444"
                }
            };

            builder.HasData(roles);
        }
    }
}
