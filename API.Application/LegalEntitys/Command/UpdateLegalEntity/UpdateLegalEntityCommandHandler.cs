using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            //Полчить LegalEntity по ИНН
            var LeInnExist = await _dbContext.LegalEntitys
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);
            //Если такое LegalEntity INN существует и не является обращаемой сущностью
            //, то выбрасываем исключение
            if (LeInnExist != null && LeInnExist.Id != request.Id)
                throw new ArgumentException($"INN: {request.INN} already exist");

            // Получить LegalEntity из базы данных
            var legalEntity = await _dbContext.LegalEntitys
                .Include(l => l.Founders)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (legalEntity == null || legalEntity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            // Получить объекты учредителей на основе массива идентификаторов FounderIds
            var founders = await _dbContext.Founders
                .Where(f => request.FounderIds.Contains(f.Id))
                .ToListAsync();

            // Добавить новых учредителей к юридическому лицу
            foreach (var founder in founders)
            {
                if (!legalEntity.Founders.Any(f => f.Id == founder.Id))
                {
                    legalEntity.Founders.Add(founder);
                    founder.LegalEntities.Add(legalEntity);
                }
            }

            // Обновить другие данные юридического лица
            legalEntity.INN = request.INN;
            legalEntity.Name = request.Name;
            legalEntity.DateUpdate = DateTime.Now;

            // Сохранить изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
