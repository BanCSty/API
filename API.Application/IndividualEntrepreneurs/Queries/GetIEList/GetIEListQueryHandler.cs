using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class GetIEListQueryHandler
        : IRequestHandler<GetIEListQuery, IEListVm>
    {
        private readonly IBaseRepository<IndividualEntrepreneur> _individualRepository;

        public GetIEListQueryHandler(IBaseRepository<IndividualEntrepreneur> individualRepository)
        {
            _individualRepository = individualRepository;
        }

        public async Task<IEListVm> Handle(GetIEListQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _individualRepository.Select()
                .Include(IE => IE.Founder)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var IELookUpDtos = entity.Select(IE => new IELookUpDto(IE)).ToList();
            var founderListVm = new IEListVm(IELookUpDtos);

            return founderListVm;
        }
    }
}
