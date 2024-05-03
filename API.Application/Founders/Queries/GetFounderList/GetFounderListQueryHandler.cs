using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Queries.GetFounderList
{
    public class GetFounderListQueryHandler
        : IRequestHandler<GetFounderListQuery, FounderListVm>
    {
        private readonly IApiDbContext _dbContext;

        public GetFounderListQueryHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FounderListVm> Handle(GetFounderListQuery request,
            CancellationToken cancellationToken)
        {
            var foundersQuery = await _dbContext.Founders.ToListAsync();

            return new FounderListVm { Founders = foundersQuery };
        }
    }
}
