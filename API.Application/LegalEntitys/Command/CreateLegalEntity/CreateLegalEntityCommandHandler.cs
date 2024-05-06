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

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommandHandler
        : IRequestHandler<CreateLegalEntityCommand, Guid>
    {
        private readonly IApiDbContext _dbContext;

        public CreateLegalEntityCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateLegalEntityCommand request, CancellationToken cancellationToken)
        {
            //Полчить LegalEntity по ИНН
            var LeInnExist = await _dbContext.LegalEntitys
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);
            //Если такое LegalEntity INN существует, то выбрасываем исключение
            if (LeInnExist != null)
                throw new ArgumentException($"INN: {request.INN} already exist");

            // Создать новое юридическое лицо
            var legalEntity = new LegalEntity
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.UtcNow,
            };

            // Получить объекты учредителей на основе массива идентификаторов FounderIds
            var founders = await _dbContext.Founders
                .Where(f => request.FounderIds.Contains(f.Id))
                .ToListAsync();

            // Добавить учредителей к юридическому лицу
            foreach (var founder in founders)
            {
                legalEntity.Founders.Add(founder);
                founder.LegalEntities.Add(legalEntity);
            }

            // Добавить новое юридическое лицо в контекст данных
            await _dbContext.LegalEntitys.AddAsync(legalEntity);
            _dbContext.Founders.UpdateRange(founders);

            // Сохранить изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            return legalEntity.Id;
        }
    }
}
