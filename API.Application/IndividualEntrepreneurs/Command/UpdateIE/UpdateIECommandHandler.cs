using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommandHandler
        : IRequestHandler<UpdateIECommand>
    {
        private readonly IApiDbContext _dbContext;

        public UpdateIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateIECommand request,
            CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.IndividualEntrepreneurs.FirstOrDefaultAsync
                (LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            entity.Name = request.Name;
            entity.INN = request.INN;
            entity.DateUpdate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
