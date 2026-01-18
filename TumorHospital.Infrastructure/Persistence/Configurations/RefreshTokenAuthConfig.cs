using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenAuthConfig : IEntityTypeConfiguration<RefreshTokenAuth>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenAuth> builder)
        {
            builder.HasKey(rfa => rfa.UserId);
        }
    }
}
