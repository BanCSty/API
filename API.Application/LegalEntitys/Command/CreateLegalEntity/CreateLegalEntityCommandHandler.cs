using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommandHandler
        : IRequestHandler<CreateLegalEntityCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IBaseRepository<LegalEntity> _legaEntityRepository;

        public CreateLegalEntityCommandHandler(IBaseRepository<LegalEntity> legaEntityRepository
            , IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork)
        {
            _legaEntityRepository = legaEntityRepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateLegalEntityCommand request, CancellationToken cancellationToken)
        {
            //Полчить LegalEntity по ИНН
            var LeInnExist = await _legaEntityRepository.Select()
                .FirstOrDefaultAsync(LE => LE.INN == request.INN, cancellationToken);
            //Если такое LegalEntity INN существует, то выбрасываем исключение
            if (LeInnExist != null)
                throw new ArgumentException($"INN: {request.INN} already exist");

            // Создать новое юридическое лицо
            var legalEntity = new LegalEntity
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.UtcNow,
            };

            // Получить объекты учредителей на основе массива идентификаторов FounderIds
            var founders = await _founderRepository.Select()
                .Include(f => f.LegalEntities)
                .Where(f => request.FounderIds.Contains(f.Id))
                .ToListAsync();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Добавить учредителей к юридическому лицу и ЮЛ к учредителям
                    foreach (var founder in founders)
                    {
                        legalEntity.Founders.Add(founder);
                        founder.LegalEntities.Add(legalEntity);

                        // Обновить состояние сущности
                        _founderRepository.Entry(founder).State = EntityState.Modified;
                    }

                    // Добавить новое юридическое лицо в контекст данных
                    await _legaEntityRepository.Create(legalEntity, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            return legalEntity.Id;
        }
    }
}
