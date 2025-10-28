using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
