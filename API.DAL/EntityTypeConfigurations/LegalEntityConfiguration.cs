using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DAL.EntityTypeConfigurations
{
    public class LegalEntityConfiguration : IEntityTypeConfiguration<LegalEntity>
    {
        public void Configure(EntityTypeBuilder<LegalEntity> builder)
        {
            builder.HasKey(LE => LE.Id);
            builder.HasIndex(LE => LE.Id).IsUnique();
            builder.Property(LE => LE.INN).IsRequired();
            builder.Property(LE => LE.Name).HasMaxLength(30).IsRequired();
            builder.Property(LE => LE.DateCreate).IsRequired();
            builder.Property(LE => LE.DateUpdate).IsRequired();
            builder.Property(LE => LE.FounderId).IsRequired();


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
                    .HasForeignKey(lf => lf.FounderId),
                j => j
                    .HasOne(lf => lf.LegalEntity)
                    .WithMany()
                    .HasForeignKey(lf => lf.LegalEntityId),
                j =>
                {
                    j.HasKey(lf => new { lf.LegalEntityId, lf.FounderId });
                    j.ToTable("LegalEntityFounder");
                }
            );
        }
    }
}
