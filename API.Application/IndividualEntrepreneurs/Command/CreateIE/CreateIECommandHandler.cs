using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommandHandler
        : IRequestHandler<CreateIECommand, Guid>
    {
        private readonly IApiDbContext _dbContext;

        public CreateIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateIECommand request, CancellationToken cancellationToken)
        {
            //Получим сущность ИП по ИНН
            var IeInnExists = await _dbContext.IndividualEntrepreneurs
                .FirstOrDefaultAsync(IE => IE.INN == request.INN, cancellationToken);
            //Если Founder INN уже существует, то выбрасываем исключение.
            //Для предотвращения данных ИП с одинаковыми ИНН
            if (IeInnExists != null)
                throw new ArgumentException($"INN: {request.INN} already used");

            // Находим учредителя по его Id
            var founder = await _dbContext.Founders.FindAsync(request.FounderId);
            if (founder == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.FounderId);
            }

            //Подгрузим ИП учредителя(если есть)
            _dbContext.Entry(founder).Reference(f => f.IndividualEntrepreneur).Load();

            // Создаем нового индивидуального предпринимателя
            var individualEntrepreneur = new IndividualEntrepreneur
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.Now,
                DateUpdate = null,
            };

            // Добавляем созданного индивидуального предпринимателя к учредителю
            founder.IndividualEntrepreneur = individualEntrepreneur;

            //добавляем учредителя к индивидуальному предпринимателю 
            individualEntrepreneur.Founder = founder;

            // Добавляем индивидуального предпринимателя в контекст базы данных
            await _dbContext.IndividualEntrepreneurs.AddAsync(individualEntrepreneur);

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Возвращаем Id нового индивидуального предпринимателя
            return individualEntrepreneur.Id;
        }
    }
}
