using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class VolunteerDonationConfig : IEntityTypeConfiguration<VolunteerDonation>
    {
        public void Configure(EntityTypeBuilder<VolunteerDonation> builder)
        {
            builder.HasKey(vd => vd.Id);

            builder.Property(vd => vd.VolunteerName)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(vd => vd.Email)
                .HasMaxLength(100);

            builder.Property(vd => vd.Phone)
                .HasMaxLength(60);

            builder.Property(vd => vd.AmountDonated)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(vd => vd.DonationDate)
                .HasDefaultValueSql("GETDATE()");

            builder
                .HasOne(vd => vd.CharityNeed)
                .WithMany(cn => cn.VolunteerDonations)
                .HasForeignKey(vd => vd.CharityNeedId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
