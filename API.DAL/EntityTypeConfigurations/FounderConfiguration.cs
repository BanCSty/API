using API.Domain;
using API.Domain.ValueObjects;
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

            // Настройка Complex Type INN
            builder.Property(founder => founder.INN)
                .HasConversion(
                    inn => inn.Value,
                    value => new INN(value)
                )
                .IsRequired()
                .HasColumnName("INN");

            builder.OwnsOne(founder => founder.FullName, fullName =>
            {
                fullName.Property(f => f.FirstName).HasMaxLength(20).IsRequired();
                fullName.Property(f => f.LastName).HasMaxLength(20).IsRequired();
                fullName.Property(f => f.MiddleName).HasMaxLength(20).IsRequired();
            });

            builder.HasOne(f => f.IndividualEntrepreneur)
                .WithOne(ie => ie.Founder)
                .HasForeignKey<IndividualEntrepreneur>(ie => ie.FounderINN);
        }
    }
}
