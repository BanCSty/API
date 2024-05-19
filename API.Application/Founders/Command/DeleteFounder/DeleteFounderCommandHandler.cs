using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.DeleteFounder
{
    /*
     * 
     *  Deletion behavior for founders:
     *  I opted for an behavior where it is allowed 
     *  for a legal entity to have no founders, as it is straightforward to implement. 
     *  However, I understand that in reality, one could configure a trigger in the database
     *  or handle the removal of a legal entity when all its founders are removed 
     *  at the application level.
     *  
     *  Поведение удаление учредителя:
     *  Я выбрал поведение в котором допускается, что у Юридического лица 
     *  может не быть учредителей т.к. она проста в реализации. 
     *  Но я так же понимаю, что по факту можно настроить триггер 
     *  в БД или же на уровне приложения удалять Юридическое лицо 
     *  у которого нет учредителей.
     * 
     */

    public class DeleteFounderCommandHandler
        : IRequestHandler<DeleteFounderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IBaseRepository<LegalEntity> _LegalEntityRepository;
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;

        public DeleteFounderCommandHandler(IBaseRepository<Founder> founderRepository, 
            IBaseRepository<LegalEntity> LegalEntityRepository, 
            IBaseRepository<IndividualEntrepreneur> IERepository,
            IUnitOfWork unitOfWork)
        {
            _founderRepository = founderRepository;
            _IERepository = IERepository;
            _LegalEntityRepository = LegalEntityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteFounderCommand request,
            CancellationToken cancellationToken)
        {
            var founder = await _founderRepository.Select()
                .FirstOrDefaultAsync(f => f.INN == request.INN, cancellationToken);

            if (founder == null)
            {
                throw new NotFoundException(nameof(Founder), request.INN);
            }

            //Подгружаем Юр. лица
            _founderRepository.Entry(founder).Collection(a => a.LegalEntities).Load();


            //Подгружаем ИП
            _founderRepository.Entry(founder).Reference(a => a.IndividualEntrepreneur).Load();


            var legalEntities = await _LegalEntityRepository.Select()
                .Where(l => l.Founders.Any(f => f.INN == request.INN))
                .ToListAsync(cancellationToken);

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    if (legalEntities != null)
                    {
                        foreach (var legalEntity in legalEntities)
                        {
                            // Удаляем учредителя из списка учредителей юридического лица
                            legalEntity.RemoveFounder(founder);
                        }
                    }

                    //Удаление ИП
                    if (founder.IndividualEntrepreneur != null)
                        await _IERepository.Delete(founder.IndividualEntrepreneur, cancellationToken);

                    // Удаление учредителя
                    await _founderRepository.Delete(founder, cancellationToken);

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
