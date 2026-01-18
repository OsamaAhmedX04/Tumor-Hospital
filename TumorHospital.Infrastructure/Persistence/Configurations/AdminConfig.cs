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
        }
    }
}
