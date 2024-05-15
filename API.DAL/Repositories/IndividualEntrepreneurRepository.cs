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

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.IndividualEntrepreneurs.FirstOrDefaultAsync(LE => LE.Id == id, cancellationToken);
            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<IndividualEntrepreneur>> Select(CancellationToken cancellationToken)
        {
            return await _dbContext.IndividualEntrepreneurs.ToListAsync(cancellationToken);
        }

        public async Task Update(IndividualEntrepreneur entity, CancellationToken cancellationToken)
        {
            _dbContext.IndividualEntrepreneurs.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
