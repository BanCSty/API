using API.Application.Common.Exceptions;
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
            // Находим удаляемую сущность LegalEntity
            var entity = await _dbContext.LegalEntitys
                .Include(le => le.Founders) // Включаем связанные учредители
                .FirstOrDefaultAsync(le => le.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            // Удаляем ссылку на удаляемую сущность LegalEntity из каждой сущности Founder
            foreach (var founder in entity.Founders)
            {
                founder.LegalEntities.Remove(entity);
            }

            // Удаляем саму сущность LegalEntity
            _dbContext.LegalEntitys.Remove(entity);

            // Сохраняем изменения
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Возвращаем пустой ответ (для тестирования)
            return Unit.Value;
        }
    }
}
