using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class GetFounderListQueryHandler
        : IRequestHandler<GetFounderListQuery, FounderListVm>
    {
        private readonly IBaseRepository<Founder> _founderRepository;

        public GetFounderListQueryHandler(IBaseRepository<Founder> founderRepository)
        {
            _founderRepository = founderRepository;
        }

        public async Task<FounderListVm> Handle(GetFounderListQuery request,
            CancellationToken cancellationToken)
        {
            var founderEntitys = await _founderRepository.Select()
                .Include(f => f.LegalEntities)
                .Include(f => f.IndividualEntrepreneur)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var founderLookUpDtos = founderEntitys.Select(founder => new FounderLookUpDto(founder)).ToList();
            var founderListVm = new FounderListVm(founderLookUpDtos);

            return founderListVm;
        }
    }
}
