using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
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
            var founderINNExists = await _founderRepository.Select()
                .FirstOrDefaultAsync(f => f.INN == request.INN, cancellationToken);

            //Если Founder с таким INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founderINNExists != null && founderINNExists.Id != request.Id)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            //Получить Founder по Id request
            var founder = await _founderRepository.Select()
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (founder == null || founder.Id != request.Id)
            {
                throw new NotFoundException(nameof(Founder), request.Id);
            }


            //Подгрузим данные ИП
            _founderRepository.Entry(founder).Reference(f => f.IndividualEntrepreneur).Load();

            //Подгрузим данные ЮЛ
            _founderRepository.Entry(founder).Collection(f => f.LegalEntities).Load();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    //Если массив Guid Юр. лиц не пуст, то обновляем данные
                    if (request.LegalEntityIds != null)
                    {
                        // Получить объекты ЮЛ на основе массива идентификаторов LegalEntityIds [UpdateFounderCommand]
                        var legalEntitys = await _LegalEntityRepository.Select()
                            .Include(LE => LE.Founders)
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
                    if (request.IndividualEntrepreneurId != Guid.Empty)
                    {
                        //Получим объект ИП по переданному в запросе Id
                        var IndividualEntrepreneurEntity = await _IERepository.Select()
                            .FirstOrDefaultAsync(IE => IE.Id == request.IndividualEntrepreneurId);

                        if (IndividualEntrepreneurEntity == null)
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
