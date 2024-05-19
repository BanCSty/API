using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    public class GetLegalEntityDetailsQueryHandler
        : IRequestHandler<GetLegalEntityDetailsQuery, LegalEntityDetailsVm>
    {
        private readonly IBaseRepository<LegalEntity> _legalEntityRepository;

        public GetLegalEntityDetailsQueryHandler(IBaseRepository<LegalEntity> legalEntityRepository)
        {
            _legalEntityRepository = legalEntityRepository;
        }

        public async Task<LegalEntityDetailsVm> Handle
            (GetLegalEntityDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity =
                await _legalEntityRepository.Select()
                .Include(le => le.Founders)
                .AsNoTracking()
                .FirstOrDefaultAsync(LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            return new LegalEntityDetailsVm(entity);
        }
    }
}
