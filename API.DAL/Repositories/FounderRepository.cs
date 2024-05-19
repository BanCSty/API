using API.DAL.Interfaces;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.DAL.Repositories
{
    public class FounderRepository : IBaseRepository<Founder>
    {
        private readonly ApiDbContext _dbContext;

        public FounderRepository(ApiDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task Create(Founder entity, CancellationToken cancellationToken)
        {
            await _dbContext.Founders.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders.FirstOrDefaultAsync(f => f.Id == id);

            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Founder> Select()
        {
            return _dbContext.Founders.AsQueryable();
        }

        public async Task Update(Founder entity, CancellationToken cancellationToken)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public EntityEntry<Founder> Entry(Founder entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return _dbContext.Entry(entity);
        }
    }
}
