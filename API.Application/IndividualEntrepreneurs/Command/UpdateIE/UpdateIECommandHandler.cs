using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
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
            var IeInnExists = await _IERepository.Select()
                .FirstOrDefaultAsync(IE => IE.INN == request.INN, cancellationToken);
            //Если такой INN уже используется и он не равен исходной сущности,
            //выбрасываем исключение. Для предотвращения данных ИП с одинаковыми ИНН
            if (IeInnExists != null && IeInnExists.Id != request.Id)
                throw new ArgumentException($"INN: {request.INN} already used");

            var entity = await _IERepository.Select()
                .FirstOrDefaultAsync(ie => ie.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            _IERepository.Entry(entity).Reference(IE => IE.Founder).Load();


            //Находим учредителей, на которых хотим записать ИП
            var founder = await _founderRepository.Select()
                .Include(f => f.IndividualEntrepreneur)
                .FirstOrDefaultAsync(f => f.Id == request.FounderId, cancellationToken);

            using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // Обновляем данные IndividualEntrepreneur
                    entity.Name = request.Name;
                    entity.INN = request.INN;
                    entity.DateUpdate = DateTime.Now;
                    entity.FounderId = request.FounderId;

                    if (founder == null || founder.IndividualEntrepreneur != null)
                        throw new ArgumentException($"Founder {founder.Id} not found or has already an Individual Entrepreneur");

                    //Если учредитель существует и у него нет активных ИП то присваиваем ему сущность ИП
                    founder.IndividualEntrepreneur = entity;

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
