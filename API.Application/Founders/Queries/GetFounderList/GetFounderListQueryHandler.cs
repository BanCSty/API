using API.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetFounderListQueryHandler(IApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<FounderListVm> Handle(GetFounderListQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .AsNoTracking()
                .ProjectTo<FounderLookUpDto>(_mapper.ConfigurationProvider)//Проецирует коллекцию в соотв. с конфигур
                .ToListAsync(cancellationToken);

            return _mapper.Map<FounderListVm>(entity);
        }
    }
}
