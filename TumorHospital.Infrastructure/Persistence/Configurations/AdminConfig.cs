using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.ApplicationUserId);

            builder
                .HasOne(a => a.User).WithMany(u => u.Admins)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.IsSuperAdmin).HasDefaultValue(false);
            builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Admin
                {
                    ApplicationUserId = "b999c3a1-4e99-4d8a-9f99-2c997b1e3d99",
                    IsSuperAdmin = true,
                }
            );
        }
    }
}
