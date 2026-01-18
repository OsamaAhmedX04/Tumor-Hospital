using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Persistence.Configurations
{
    public class FAQConfig : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Question)
                   .IsRequired();

            builder.Property(f => f.Answer)
                   .IsRequired();
        }
    }
}
