using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQueryHandler
        : IRequestHandler<GetIEDetailsQuery, IndividualEntrepreneur>
    {
        private readonly IApiDbContext _dbContext;

        public GetIEDetailsQueryHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IndividualEntrepreneur> Handle
            (GetIEDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.IndividualEntrepreneurs.FirstOrDefaultAsync
                (LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            return entity;
        }
    }
}
