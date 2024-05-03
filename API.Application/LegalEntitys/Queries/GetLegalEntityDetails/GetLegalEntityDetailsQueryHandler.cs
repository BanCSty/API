using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    public class GetLegalEntityDetailsQueryHandler
        : IRequestHandler<GetLegalEntityDetailsQuery, LegalEntityDetailsVm>
    {
        private readonly IApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetLegalEntityDetailsQueryHandler(IApiDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<LegalEntityDetailsVm> Handle
            (GetLegalEntityDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.LegalEntitys.FirstOrDefaultAsync
                (LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            return _mapper.Map<LegalEntityDetailsVm>(entity);
        }
    }
}
