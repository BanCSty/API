using API.Application.Interfaces;
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

        public GetIEListQueryHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEListVm> Handle(GetIEListQuery request,
            CancellationToken cancellationToken)
        {
            var IEListQuery = await _dbContext.IndividualEntrepreneurs.ToListAsync();

            return new IEListVm { IEList = IEListQuery };
        }
    }
}
