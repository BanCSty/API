using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using API.Domain.ValueObjects;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IBaseRepository<LegalEntity> _LegalEntityRepository;
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;

        public UpdateFounderCommandHandler(IBaseRepository<Founder> founderRepository,
            IBaseRepository<LegalEntity> LegalEntityRepository,
            IBaseRepository<IndividualEntrepreneur> IERepository,
            IUnitOfWork unitOfWork)
        {
            _founderRepository = founderRepository;
            _IERepository = IERepository;
            _LegalEntityRepository = LegalEntityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFounderCommand request,
            CancellationToken cancellationToken)
        {
            //Получаем Founder по reqiest INN
            var founder = await _founderRepository.Select()
                .FirstOrDefaultAsync(f => f.INN == request.INN, cancellationToken);

            //Если Founder с таким INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founder != null && founder.INN != request.INN)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            if (founder == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.INN);
            }


            //Подгрузим данные ИП
            _founderRepository.Entry(founder).Reference(f => f.IndividualEntrepreneur).Load();

            //Подгрузим данные ЮЛ
            _founderRepository.Entry(founder).Collection(f => f.LegalEntities).Load();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    //Если массив INN Юр. лиц не пуст, то обновляем данные
                    if (request.LegalEntityINNs != null)
                    {
                        // Получить объекты ЮЛ на основе массива идентификаторов LegalEntityINNs [UpdateFounderCommand]
                        var legalEntitys = await _LegalEntityRepository.Select()
                            .Include(LE => LE.Founders)
                            .Where(LE => request.LegalEntityINNs.Contains(LE.INN))
                            .ToListAsync();

                        if (legalEntitys == null)
                            throw new NotFoundException(nameof(LegalEntity), request.LegalEntityINNs);

                        // Добавить новых ЮЛ к учредителю и учредителей к ЮЛ
                        foreach (var legalEntity in legalEntitys)
                        {
                            if (!founder.LegalEntities.Any(LE => LE.INN == legalEntity.INN))
                            {
                                legalEntity.AddFounder(founder);
                                founder.AddLegalEntity(legalEntity);
                            }
                        }
                    }

                    //Обновление ссылки на ИП, если не пуст IndividualEntrepreneurINN запроса
                    if (request.IndividualEntrepreneurINN != null)
                    {
                        //Получим объект ИП по переданному в запросе Id
                        var IndividualEntrepreneurEntity = await _IERepository.Select()
                            .FirstOrDefaultAsync(IE => IE.INN == request.IndividualEntrepreneurINN);

                        if (IndividualEntrepreneurEntity == null)
                            throw new NotFoundException(nameof(IndividualEntrepreneur), request.IndividualEntrepreneurINN);

                        //Добавим к учредителю 
                        founder.AssignIndividualEntrepreneur(IndividualEntrepreneurEntity);
                    }

                    // Обновление данные учредителя
                    founder.UpdateFullName(new FullName(request.FirstName, request.LastName, request.MiddleName));
                    
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
