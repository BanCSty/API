using API.Domain;
using API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.EntityTypeConfigurations
{
    public class LegalEntityConfiguration : IEntityTypeConfiguration<LegalEntity>
    {
        public void Configure(EntityTypeBuilder<LegalEntity> builder)
        {
            builder.HasKey(LE => LE.INN);
            builder.HasIndex(LE => LE.INN).IsUnique();
            builder.Property(LE => LE.Name).HasMaxLength(30).IsRequired();

            // Настройка Complex Type INN
            builder.Property(founder => founder.INN)
                .HasConversion(
                    inn => inn.Value,
                    value => new INN(value)
                )
                .IsRequired()
                .HasColumnName("INN");

            /*
             * отношение "многие ко многим" между Founder и LegalEntity позволяет описать связь,
             * где каждый учредитель может быть связан с несколькими юридическими лицами, 
             * а каждое юридическое лицо может иметь несколько учредителей
             */

            builder
            .HasMany(legalEntity => legalEntity.Founders) // У каждого юридического лица может быть несколько учредителей
            .WithMany(founder => founder.LegalEntities) // Каждый учредитель может быть связан с несколькими юридическими лицами
            .UsingEntity<LegalEntityFounder>(
                j => j
                    .HasOne(lf => lf.Founder)
                    .WithMany()
                    .HasForeignKey(lf => lf.FounderINN),
                j => j
                    .HasOne(lf => lf.LegalEntity)
                    .WithMany()
                    .HasForeignKey(lf => lf.LegalEntityINN),
                j =>
                {
                    j.HasKey(lf => new { lf.LegalEntityINN, lf.FounderINN });
                    j.ToTable("LegalEntityFounder");
                }
            );
        }
    }
}
