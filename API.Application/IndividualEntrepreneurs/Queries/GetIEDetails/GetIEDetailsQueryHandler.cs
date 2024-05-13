using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQueryHandler
        : IRequestHandler<GetIEDetailsQuery, IEDetailsVm>
    {
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetIEDetailsQueryHandler(IApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEDetailsVm> Handle
            (GetIEDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.IndividualEntrepreneurs
                .Include(f => f.Founder)
                .AsNoTracking()
                .FirstOrDefaultAsync(LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            return _mapper.Map<IEDetailsVm>(entity); ;
        }
    }
}
