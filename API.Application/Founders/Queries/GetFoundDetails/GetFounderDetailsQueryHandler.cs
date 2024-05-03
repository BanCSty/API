using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQueryHandler
        : IRequestHandler<GetFounderDetailsQuery, Founder>
    {
        private readonly IApiDbContext _dbContext;

        public GetFounderDetailsQueryHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Founder> Handle(GetFounderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .FirstOrDefaultAsync(founder =>
                founder.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }

            return entity;
        }
    }
}
