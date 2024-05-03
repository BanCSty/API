using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommandHandler
        : IRequestHandler<DeleteLegalEntityCommand>
    {
        private readonly IApiDbContext _dbContext;

        public DeleteLegalEntityCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteLegalEntityCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            _dbContext.Founders.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //Пустой ответ(для тестирования)
            return Unit.Value;
        }
    }
}
