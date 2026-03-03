using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class VideoCallConfig : IEntityTypeConfiguration<VideoCall>
    {
        public void Configure(EntityTypeBuilder<VideoCall> builder)
        {
            builder.HasKey(vc => vc.Id);

            builder.Property(vc => vc.CallerId)
                .IsRequired();

            builder.Property(vc => vc.ReceiverId)
                .IsRequired();

            builder.Property(vc => vc.StartedAt)
                .IsRequired();

            builder.Property(vc => vc.IsActive)
                .IsRequired();

            builder.Property(vc => vc.Status)
               .HasConversion<string>()
               .IsRequired()
               .HasMaxLength(20);

            builder.HasOne(vc => vc.Patient)
                .WithMany(p => p.VideoCalls)
                .HasForeignKey(vc => vc.CallerId)
                .HasPrincipalKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vc => vc.Doctor)
                .WithMany(d => d.VideoCalls)
                .HasForeignKey(vc => vc.ReceiverId)
                .HasPrincipalKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(vc => vc.Appointment)
                .WithMany(a => a.VideoCalls)
                .HasForeignKey(vc => vc.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(vc => new { vc.AppointmentId, vc.IsActive })
                   .HasFilter("[IsActive] = 1")
                   .IsUnique();

        }
    }
}
