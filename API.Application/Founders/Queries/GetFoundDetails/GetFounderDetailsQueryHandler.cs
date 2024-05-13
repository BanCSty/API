using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQueryHandler
        : IRequestHandler<GetFounderDetailsQuery, FounderDetailsVm>
    {
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetFounderDetailsQueryHandler(IApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<FounderDetailsVm> Handle(GetFounderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .AsNoTracking()
                .Include(f => f.LegalEntities)
                .Include(f => f.IndividualEntrepreneur)
                .FirstOrDefaultAsync
                (founder =>founder.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }

            return _mapper.Map<FounderDetailsVm>(entity);
        }
    }
}
