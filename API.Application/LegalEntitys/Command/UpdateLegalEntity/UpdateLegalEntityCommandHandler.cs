using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.UpdateLegalEntity
{
    public class UpdateLegalEntityCommandHandler
        : IRequestHandler<UpdateLegalEntityCommand>
    {
        private readonly IApiDbContext _dbContext;

        public UpdateLegalEntityCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateLegalEntityCommand request,
            CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.LegalEntitys.FirstOrDefaultAsync
                (LE => LE.Id == request.Id, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            entity.Name = request.Name;
            entity.INN = request.INN;
            entity.DateUpdate = DateTime.Now;

            if(request.FounderId != null)
                entity.FounderId = request.FounderId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
