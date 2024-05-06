using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommandHandler
        : IRequestHandler<UpdateIECommand>
    {
        private readonly IApiDbContext _dbContext;

        public UpdateIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateIECommand request,
            CancellationToken cancellationToken)
        {
            //Получаем IndividualEntrepreneur по INN 
            var IeInnExists = await _dbContext.IndividualEntrepreneurs
                .FirstOrDefaultAsync(IE => IE.INN == request.INN, cancellationToken);
            //Если такой INN уже используется и он не равен исходной сущности,
            //выбрасываем исключение. Для предотвращения данных ИП с одинаковыми ИНН
            if (IeInnExists != null && IeInnExists.Id != request.Id)
                throw new ArgumentException($"INN: {request.INN} already used");

            var entity = await _dbContext.IndividualEntrepreneurs.FirstOrDefaultAsync(ie => ie.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            // Обновляем данные IndividualEntrepreneur
            entity.Name = request.Name;
            entity.INN = request.INN;
            entity.DateUpdate = DateTime.Now;


            //Находим учредителей, на которых хотим записать ИП
            var founder = await _dbContext.Founders.FirstOrDefaultAsync(f => f.Id == request.FounderId);

            //Если учредитель существует и у него нет активных ИП то присваиваем ему сущность ИП
            if (founder != null && founder.IndividualEntrepreneur == null)
            {
                founder.IndividualEntrepreneur = entity;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
