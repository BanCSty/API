using API.DAL.Interfaces;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<LegalEntity>> Select(CancellationToken cancellationToken)
        {
            return await _dbContext.LegalEntitys.ToListAsync(cancellationToken);
        }

        public async Task Update(LegalEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.LegalEntitys.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
