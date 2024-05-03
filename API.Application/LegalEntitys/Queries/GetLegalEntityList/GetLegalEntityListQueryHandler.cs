using API.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityList
{
    public class GetLegalEntityListQueryHandler
        : IRequestHandler<GetLegalEntityListQuery, LegalEntityListVm>
    {
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetLegalEntityListQueryHandler(IApiDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<LegalEntityListVm> Handle
            (GetLegalEntityListQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.LegalEntitys
                .ProjectTo<LegalEntityLookUpDto>(_mapper.ConfigurationProvider)//Проецирует коллекцию в соотв. с конфигур
                .ToListAsync(cancellationToken);

            return _mapper.Map<LegalEntityListVm>(entity);
        }
    }
}
