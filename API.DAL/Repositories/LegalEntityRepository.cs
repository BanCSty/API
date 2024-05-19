using API.DAL.Interfaces;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Repositories
{
    public class LegalEntityRepository : IBaseRepository<LegalEntity>
    {
        private readonly ApiDbContext _dbContext;

        public LegalEntityRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(LegalEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.LegalEntitys.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.LegalEntitys.FirstOrDefaultAsync(LE => LE.Id == id, cancellationToken);
            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<LegalEntity> Select()
        {          
            return _dbContext.LegalEntitys.AsQueryable();
        }

        public async Task Update(LegalEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.LegalEntitys.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public EntityEntry<LegalEntity> Entry(LegalEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbContext.Entry(entity);
        }
    }
}
