using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.EntityTypeConfigurations
{
    public class FounderConfiguration : IEntityTypeConfiguration<Founder>
    {
        public void Configure(EntityTypeBuilder<Founder> builder)
        {
            builder.HasKey(founder => founder.Id);
            builder.HasIndex(founder => founder.Id).IsUnique();
            builder.Property(founder => founder.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(founder => founder.LastName).HasMaxLength(20).IsRequired();
            builder.Property(founder => founder.SecondName).HasMaxLength(20).IsRequired();
            builder.Property(founder => founder.DateCreate).IsRequired();
            builder.Property(founder => founder.DateUpdate).IsRequired();
        }
    }
}
