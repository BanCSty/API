using API.DAL.Interfaces;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<Founder>> Select(CancellationToken cancellationToken)
        {
            return await _dbContext.Founders.ToListAsync(cancellationToken);
        }

        public async Task Update(Founder entity, CancellationToken cancellationToken)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
