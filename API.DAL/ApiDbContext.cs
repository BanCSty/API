
using API.DAL.EntityTypeConfigurations;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Founder> Founders { get; set; }
        public DbSet<LegalEntity> LegalEntitys { get ; set ; }
        public DbSet<IndividualEntrepreneur> IndividualEntrepreneurs { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FounderConfiguration());
            builder.ApplyConfiguration(new IndividualEntrepreneurConfiguration());
            builder.ApplyConfiguration(new LegalEntityConfiguration());
            base.OnModelCreating(builder);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
