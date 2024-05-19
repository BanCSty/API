using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
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

        public async Task<Guid> Handle(CreateIECommand request, CancellationToken cancellationToken)
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
                .FirstOrDefaultAsync(f => f.Id == request.FounderId);
            if (founder == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.FounderId);
            }

            // Создаем нового индивидуального предпринимателя
            var individualEntrepreneur = new IndividualEntrepreneur
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.Now,
                DateUpdate = null,
                FounderId = request.FounderId
            };  


            using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    if (founder == null || founder.IndividualEntrepreneur != null)
                        throw new ArgumentException($"Founder {founder.Id} already has an individual entrepreneur");                      

                    //добавляем учредителя к индивидуальному предпринимателю 
                    individualEntrepreneur.Founder = founder;

                    // Создаем индивидуального предпринимателя
                    await _IERepository.Create(individualEntrepreneur, cancellationToken);

                    // Добавляем созданного индивидуального предпринимателя к учредителю
                    founder.IndividualEntrepreneur = individualEntrepreneur;

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

            // Возвращаем Id созданного индивидуального предпринимателя
            return individualEntrepreneur.Id;
        }
    }
}
