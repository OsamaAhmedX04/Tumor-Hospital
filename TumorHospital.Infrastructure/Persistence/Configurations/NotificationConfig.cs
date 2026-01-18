using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Description)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(n => n.RouteAction)
                .IsRequired();

            builder.Property(n => n.SentAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(n => n.IsSeen)
                .HasDefaultValue(false);

            builder
                .HasOne(n => n.ApplicationUser).WithMany(u => u.Notifications)
                .HasForeignKey(n => n.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
