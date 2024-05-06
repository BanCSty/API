using API.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEList
{
    public class GetIEListQueryHandler
        : IRequestHandler<GetIEListQuery, IEListVm>
    {
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetIEListQueryHandler(IApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEListVm> Handle(GetIEListQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.IndividualEntrepreneurs
                .AsNoTracking()
                .ProjectTo<IELookUpDto>(_mapper.ConfigurationProvider)//Проецирует коллекцию в соотв. с конфигур
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEListVm>(entity);
        }
    }
}
