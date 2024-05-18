using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    public class GetLegalEntityListQueryHandler
        : IRequestHandler<GetLegalEntityListQuery, LegalEntityListVm>
    {
        private readonly IBaseRepository<LegalEntity> _legalEntityRepository;

        public GetLegalEntityListQueryHandler(IBaseRepository<LegalEntity> legalEntityRepository)
        {
            _legalEntityRepository = legalEntityRepository;
        }

        public async Task<LegalEntityListVm> Handle
            (GetLegalEntityListQuery request, CancellationToken cancellationToken)
        {
            var entity = await _legalEntityRepository.Select()
                .Include(LE => LE.Founders)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var legalEntityLookUp = entity.Select(LE => new LegalEntityLookUpDto(LE)).ToList();
            var legalEntityListVm = new LegalEntityListVm(legalEntityLookUp);

            return legalEntityListVm;
        }
    }
}
