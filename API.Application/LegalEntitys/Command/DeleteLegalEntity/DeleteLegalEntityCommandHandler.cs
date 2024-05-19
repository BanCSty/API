using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommandHandler
        : IRequestHandler<DeleteLegalEntityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IBaseRepository<LegalEntity> _legaEntityRepository;

        public DeleteLegalEntityCommandHandler(IBaseRepository<LegalEntity> legaEntityRepository
            , IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork)
        {
            _legaEntityRepository = legaEntityRepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLegalEntityCommand request,
            CancellationToken cancellationToken)
        {
            // Находим удаляемую сущность LegalEntity
            var entity = await _legaEntityRepository.Select()
                .FirstOrDefaultAsync(le => le.INN == request.INN, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(LegalEntity), request.INN);
            }

            //Подгружаем учредителей
            _legaEntityRepository.Entry(entity).Collection(LE => LE.Founders).Load();

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Удаляем ссылку на удаляемую сущность LegalEntity из каждой сущности Founder
                    foreach (var founder in entity.Founders)
                    {
                        founder.DeleteLegalEntity(entity);
                    }

                    // Удаляем саму сущность LegalEntity
                    await _legaEntityRepository.Delete(entity, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (System.Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }

            // Возвращаем пустой ответ (для тестирования)
            return Unit.Value;
        }
    }
}
