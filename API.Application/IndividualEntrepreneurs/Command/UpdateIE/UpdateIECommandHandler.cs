using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using API.Domain.ValueObjects;
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
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateIECommandHandler(IBaseRepository<IndividualEntrepreneur> IERepository, 
            IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork)
        {
            _IERepository = IERepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateIECommand request,
            CancellationToken cancellationToken)
        {
            //Получаем IndividualEntrepreneur по INN 
            var entity = await _IERepository.Select()
                .FirstOrDefaultAsync(IE => IE.INN == request.INN, cancellationToken);
            //Если такой INN уже используется и он не равен исходной сущности,
            //выбрасываем исключение. Для предотвращения данных ИП с одинаковыми ИНН
            if (entity != null && entity.INN != request.INN)
                throw new ArgumentException($"INN: {request.INN} already used");


            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.INN);
            }

            _IERepository.Entry(entity).Reference(IE => IE.Founder).Load();


            //Находим учредителей, на которых хотим записать ИП
            var founder = await _founderRepository.Select()
                .Include(f => f.IndividualEntrepreneur)
                .FirstOrDefaultAsync(f => f.INN == request.FounderINN, cancellationToken);

            using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    
                    if (founder == null || founder.IndividualEntrepreneur != null)
                        throw new ArgumentException($"Founder {founder.INN} not found or has already an Individual Entrepreneur");

                    // Обновляем данные IndividualEntrepreneur
                    entity.UpdateName(request.Name);
                    entity.AddFounder(founder);

                    //Если учредитель существует и у него нет активных ИП то присваиваем ему сущность ИП
                    founder.AssignIndividualEntrepreneur(entity);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();                   
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            return Unit.Value;
        }
    }
}
