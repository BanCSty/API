using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommandHandler
        : IRequestHandler<DeleteIECommand>
    {
        private readonly IApiDbContext _dbContext;

        public DeleteIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteIECommand request, CancellationToken cancellationToken)
        {
            // Находим IndividualEntrepreneur по Id запроса
            var entity = await _dbContext.IndividualEntrepreneurs
                .FirstOrDefaultAsync(ie => ie.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            _dbContext.Entry(entity).Reference(IE => IE.Founder).Load();

            // Находим учредителя, связанного с IndividualEntrepreneurId
            var founder = await _dbContext.Founders
                .Include(f => f.IndividualEntrepreneur)
                .SingleOrDefaultAsync(f => f.Id == entity.FounderId, cancellationToken);

            //Если есть учредитель связанный с этим ИП, то это поле в таблице учредителей будет null
            if (founder != null)
                founder.IndividualEntrepreneur = null;

            // Удаляем IndividualEntrepreneur
            _dbContext.IndividualEntrepreneurs.Remove(entity);

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Возвращаем пустой ответ
            return Unit.Value;
        }
    }
}
