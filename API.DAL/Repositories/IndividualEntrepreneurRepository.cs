using API.DAL.Interfaces;
using API.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Repositories
{
    public class IndividualEntrepreneurRepository : IBaseRepository<IndividualEntrepreneur>
    {
        private readonly ApiDbContext _dbContext;

        public IndividualEntrepreneurRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(IndividualEntrepreneur entity, CancellationToken cancellationToken)
        {
            await _dbContext.IndividualEntrepreneurs.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(IndividualEntrepreneur entity, CancellationToken cancellationToken)
        {
            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<IndividualEntrepreneur> Select()
        {
            return _dbContext.IndividualEntrepreneurs.AsQueryable();
        }

        public async Task Update(IndividualEntrepreneur entity, CancellationToken cancellationToken)
        {
            _dbContext.IndividualEntrepreneurs.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public EntityEntry<IndividualEntrepreneur> Entry(IndividualEntrepreneur entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbContext.Entry(entity);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
    }
}
