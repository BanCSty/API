using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommandHandler
        : IRequestHandler<UpdateFounderCommand>
    {
        private readonly IApiDbContext _dbContext;

        public UpdateFounderCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateFounderCommand request,
            CancellationToken cancellationToken)
        {
            //Получаем Founder по reqiest INN
            var founderExist = await _dbContext.Founders.FirstOrDefaultAsync(f => f.INN == request.INN, cancellationToken);

            //Если Founder INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founderExist != null && founderExist.Id != request.Id)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            //Получить Founder по Id request
            var founder = await _dbContext.Founders
                .Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (founder == null || founder.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }

            // Получить объекты ЮЛ на основе массива идентификаторов LegalEntityIds
            var legalEntitys = await _dbContext.LegalEntitys
                .Where(l => request.LegalEntityIds.Contains(l.Id))
                .ToListAsync();

            // Добавить новых ЮЛ к учредителю и учредителей к ЮЛ
            foreach (var legalEntity in legalEntitys)
            {
                if (!founder.LegalEntities.Any(LE => LE.Id == legalEntity.Id))
                {
                    legalEntity.Founders.Add(founder);
                    founder.LegalEntities.Add(legalEntity);
                }
            }

            // Обновить другие данные учредителя
            founder.INN = request.INN;
            founder.FirstName = request.FirstName;
            founder.LastName = request.LastName;
            founder.MiddleName = request.MiddleName;
            founder.DateUpdate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
