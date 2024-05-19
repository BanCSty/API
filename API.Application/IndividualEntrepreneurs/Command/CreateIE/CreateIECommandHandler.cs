using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using API.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommandHandler
        : IRequestHandler<CreateIECommand>
    {
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;

        public CreateIECommandHandler(IBaseRepository<IndividualEntrepreneur> IERepository, 
            IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork
            )
        {
            _IERepository = IERepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateIECommand request, CancellationToken cancellationToken)
        {
            //Получим сущность ИП по ИНН
            var IeInnExists = await _IERepository.Select()
                .FirstOrDefaultAsync(IE => IE.INN == request.INN, cancellationToken);

            //Если Founder INN уже существует, то выбрасываем исключение.
            //Для предотвращения данных ИП с одинаковыми ИНН
            if (IeInnExists != null)
                throw new ArgumentException($"INN: {request.INN} already used");

            // Находим учредителя по его Id
            var founder = await _founderRepository.Select()
                .Include(f => f.IndividualEntrepreneur)
                .FirstOrDefaultAsync(f => f.INN == request.INN);
            if (founder == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.FounderINN);
            }

            // Создаем нового индивидуального предпринимателя
            var individualEntrepreneur = new IndividualEntrepreneur
                (
                    (INN)request.INN,
                    request.Name,
                    DateTime.Now,
                    (INN)request.FounderINN
                );



            using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    if (founder == null || founder.IndividualEntrepreneur != null)
                        throw new ArgumentException($"Founder {founder.INN} already has an individual entrepreneur");

                    //добавляем учредителя к индивидуальному предпринимателю 
                    individualEntrepreneur.AddFounder(founder);

                    // Создаем индивидуального предпринимателя
                    await _IERepository.Create(individualEntrepreneur, cancellationToken);

                    // Добавляем созданного индивидуального предпринимателя к учредителю
                    founder.AssignIndividualEntrepreneur(individualEntrepreneur);

                    // Сохраняем изменения и завершаем транзакцию
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();

                }
                catch (Exception)
                {
                    // Откатываем транзакцию в случае ошибки
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            return Unit.Value;
        }
    }
}
