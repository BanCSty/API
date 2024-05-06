using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
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
            // Находим учредителя, связанного с IndividualEntrepreneurId
            var founder = await _dbContext.Founders
                .SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (founder != null)
            {
                // Удаляем связь учредителя с IndividualEntrepreneur
                _dbContext.Founders.RemoveRange(founder);
            }

            // Находим IndividualEntrepreneur
            var entity = await _dbContext.IndividualEntrepreneurs
                .FirstOrDefaultAsync(ie => ie.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            // Удаляем IndividualEntrepreneur
            _dbContext.IndividualEntrepreneurs.Remove(entity);

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Возвращаем пустой ответ
            return Unit.Value;
        }
    }
}
