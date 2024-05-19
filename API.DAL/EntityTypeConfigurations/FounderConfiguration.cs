using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.EntityTypeConfigurations
{
    public class FounderConfiguration : IEntityTypeConfiguration<Founder>
    {
        public void Configure(EntityTypeBuilder<Founder> builder)
        {
            builder.HasKey(founder => founder.INN);
            builder.HasIndex(founder => founder.INN).IsUnique();
            builder.Property(founder => founder.INN).IsRequired();
            builder.Property(founder => founder.FullName.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(founder => founder.FullName.LastName).HasMaxLength(20).IsRequired();
            builder.Property(founder => founder.FullName.MiddleName).HasMaxLength(20).IsRequired();
            
            builder.HasOne(f => f.IndividualEntrepreneur)
                .WithOne(ie => ie.Founder)
                .HasForeignKey<IndividualEntrepreneur>(ie => ie.FounderINN);
        }
    }
}
