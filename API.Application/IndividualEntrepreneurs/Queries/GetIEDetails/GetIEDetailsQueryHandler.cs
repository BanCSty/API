using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQueryHandler
        : IRequestHandler<GetIEDetailsQuery, IEDetailsVm>
    {
        private readonly IBaseRepository<IndividualEntrepreneur> _individualRepository;

        public GetIEDetailsQueryHandler(IBaseRepository<IndividualEntrepreneur> individualRepository)
        {
            _individualRepository = individualRepository;
        }

        public async Task<IEDetailsVm> Handle
            (GetIEDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity =
                await _individualRepository.Select()
                .Include(IE => IE.Founder)
                .AsNoTracking()
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);

            if (entity == null || entity.INN != request.INN)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.INN);
            }

            return new IEDetailsVm(entity);
        }
    }
}
