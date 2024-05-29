using API.Domain;
using API.Domain.ValueObjects;
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

            // Настройка Complex Type INN
            builder.Property(founder => founder.INN)
                .HasConversion(
                    inn => inn.Value,
                    value => new INN(value)
                )
                .IsRequired()
                .HasColumnName("INN");
        }
    }
}
