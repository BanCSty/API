using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
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
            var LeInnExist = await _legaEntityRepository.Select()
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);
            //Если такое LegalEntity INN существует и не является обращаемой сущностью
            //, то выбрасываем исключение
            if (LeInnExist != null && LeInnExist.Id != request.Id)
                throw new ArgumentException($"INN: {request.INN} already exist");

            // Получить LegalEntity из базы данных
            var legalEntity = await _legaEntityRepository.Select()
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (legalEntity == null || legalEntity.Id != request.Id)
            {
                throw new NotFoundException(nameof(LegalEntity), request.Id);
            }

            _legaEntityRepository.Entry(legalEntity).Collection(LE => LE.Founders).Load();

            // Получить объекты учредителей на основе массива идентификаторов FounderIds
            var founders = await _founderRepository.Select()
                .Include(f => f.LegalEntities)
                .Where(f => request.FounderIds.Contains(f.Id))
                .ToListAsync();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Добавить новых учредителей к юридическому лицу
                    foreach (var founder in founders)
                    {
                        //Если учредитель еще не связан с ЮЛ происходит привязка
                        if (!legalEntity.Founders.Any(f => f.Id == founder.Id))
                        {
                            legalEntity.Founders.Add(founder);
                            founder.LegalEntities.Add(legalEntity);
                        }
                    }

                    // Обновить другие данные юридического лица
                    legalEntity.INN = request.INN;
                    legalEntity.Name = request.Name;
                    legalEntity.DateUpdate = DateTime.Now;

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
