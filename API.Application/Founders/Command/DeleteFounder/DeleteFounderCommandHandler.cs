using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.DeleteFounder
{
    /*
     * 
     *  Deletion behavior for founders:
     *  I opted for an behavior where it is allowed 
     *  for a legal entity to have no founders, as it is straightforward to implement. 
     *  However, I understand that in reality, one could configure a trigger in the database
     *  or handle the removal of a legal entity when all its founders are removed 
     *  at the application level.
     *  
     *  Поведение удаление учредителя:
     *  Я выбрал поведение в которой допускается, что у Юридического лица 
     *  может не быть учредителей т.к. она проста в реализации. 
     *  Но я так же понимаю, что по факту можно настроить триггер 
     *  в БД или же на уровне приложения удалять Юридическое лицо 
     *  у которого нет учредителей.
     * 
     */

    public class DeleteFounderCommandHandler 
        : IRequestHandler<DeleteFounderCommand>
    {
        private readonly IApiDbContext _dbContext;

        public DeleteFounderCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteFounderCommand request, 
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == request.FounderId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Founder), request.FounderId);
            }

            // Удаление всех связанных записей LegalEntity
            _dbContext.LegalEntitys.RemoveRange(entity.LegalEntities);

            // Удаление учредителя
            _dbContext.Founders.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
