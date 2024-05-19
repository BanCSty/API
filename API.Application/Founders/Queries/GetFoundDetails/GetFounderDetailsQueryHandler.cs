using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQueryHandler
        : IRequestHandler<GetFounderDetailsQuery, FounderDetailsVm>
    {
        private readonly IBaseRepository<Founder> _founderRepository;

        public GetFounderDetailsQueryHandler(IBaseRepository<Founder> founderRepository)
        {
            _founderRepository = founderRepository;
        }

        public async Task<FounderDetailsVm> Handle(GetFounderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var founderEntity = await _founderRepository.Select()
                .Include(f => f.LegalEntities)
                .Include(f => f.IndividualEntrepreneur)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (founderEntity == null || founderEntity.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }

            var founderDetails = new FounderDetailsVm(founderEntity);

            return founderDetails;
        }
    }
}
