using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.EntityTypeConfigurations
{
    public class IndividualEntrepreneurConfiguration : IEntityTypeConfiguration<IndividualEntrepreneur>
    {
        public void Configure(EntityTypeBuilder<IndividualEntrepreneur> builder)
        {
            builder.HasKey(IE => IE.INN);
            builder.HasIndex(IE => IE.INN).IsUnique();
            builder.Property(IE => IE.Name).HasMaxLength(30).IsRequired();
            builder.Property(IE => IE.FounderINN).IsRequired();
        }
    }
}
