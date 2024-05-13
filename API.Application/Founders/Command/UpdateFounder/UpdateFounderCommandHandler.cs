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

            //Если Founder с таким INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founderExist != null && founderExist.Id != request.Id)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            //Получить Founder по Id request
            var founder = await _dbContext.Founders
                //.Include(f => f.LegalEntities)
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (founder == null || founder.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }


            //Подгрузим данные ИП
            _dbContext.Entry(founder).Reference(f => f.IndividualEntrepreneur).Load();

            //Подгрузим данные ЮЛ
            _dbContext.Entry(founder).Collection(f => f.LegalEntities).Load();

            //Если массив Guid Юр. лиц не пуст, то обновляем данные
            if (request.LegalEntityIds != null)
            {
                // Получить объекты ЮЛ на основе массива идентификаторов LegalEntityIds [UpdateFounderCommand]
                var legalEntitys = await _dbContext.LegalEntitys
                    .Where(LE => request.LegalEntityIds.Contains(LE.Id))
                    .ToListAsync();

                if (legalEntitys == null)
                    throw new NotFoundException(nameof(LegalEntity), request.LegalEntityIds);

                // Добавить новых ЮЛ к учредителю и учредителей к ЮЛ
                foreach (var legalEntity in legalEntitys)
                {
                    if (!founder.LegalEntities.Any(LE => LE.Id == legalEntity.Id))
                    {
                        legalEntity.Founders.Add(founder);
                        founder.LegalEntities.Add(legalEntity);
                    }
                }
            }

            //Обновление ссылки на ИП, если не пуст IndividualEntrepreneurId запроса
            if (request.IndividualEntrepreneurId != null)
            {
                //Получим объект ИП по переданному в запросе Id
                var IndividualEntrepreneurEntity = await _dbContext.IndividualEntrepreneurs
                    .FirstOrDefaultAsync(IE => IE.Id == request.IndividualEntrepreneurId);

                if(IndividualEntrepreneurEntity == null)
                    throw new NotFoundException(nameof(IndividualEntrepreneur), request.IndividualEntrepreneurId);

                //Добавим к учредителю 
                founder.IndividualEntrepreneur = IndividualEntrepreneurEntity;
            }

            // Обновление данные учредителя
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
