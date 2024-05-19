using API.DAL.Interfaces;
using API.Domain;
using API.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.UpdateLegalEntity
{
    public class UpdateLegalEntityCommandHandler
        : IRequestHandler<UpdateLegalEntityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IBaseRepository<LegalEntity> _legaEntityRepository;

        public UpdateLegalEntityCommandHandler(IBaseRepository<LegalEntity> legaEntityRepository
            , IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork)
        {
            _legaEntityRepository = legaEntityRepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLegalEntityCommand request,
            CancellationToken cancellationToken)
        {
            //Полчить LegalEntity по ИНН
            var legalEntity = await _legaEntityRepository.Select()
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);
            //Если такое LegalEntity INN существует и не является обращаемой сущностью
            //, то выбрасываем исключение
            if (legalEntity != null && legalEntity.INN != request.INN)
                throw new ArgumentException($"INN: {request.INN} already exist");

            _legaEntityRepository.Entry(legalEntity).Collection(LE => LE.Founders).Load();

            // Получить объекты учредителей на основе массива идентификаторов FounderIds
            var founders = await _founderRepository.Select()
                .Include(f => f.LegalEntities)
                .Where(f => request.FounderINNs.Contains(f.INN))
                .ToListAsync();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Добавить новых учредителей к юридическому лицу
                    foreach (var founder in founders)
                    {
                        //Если учредитель еще не связан с ЮЛ происходит привязка
                        if (!legalEntity.Founders.Any(f => f.INN == founder.INN))
                        {
                            legalEntity.AddFounder(founder);
                            founder.AddLegalEntity(legalEntity);
                        }
                    }

                    // Обновить другие данные юридического лица
                    legalEntity.UpdateName(request.Name);

                    // Сохранить изменения в базе данных
                    await _legaEntityRepository.Update(legalEntity, cancellationToken);

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
