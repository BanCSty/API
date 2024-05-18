using API.Application.Common.Exceptions;
using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommandHandler
        : IRequestHandler<DeleteIECommand>
    {
        private readonly IBaseRepository<IndividualEntrepreneur> _IERepository;
        private readonly IBaseRepository<Founder> _founderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteIECommandHandler(IBaseRepository<IndividualEntrepreneur> IERepository, 
            IBaseRepository<Founder> founderRepository,
            IUnitOfWork unitOfWork)
        {
            _IERepository = IERepository;
            _founderRepository = founderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteIECommand request, CancellationToken cancellationToken)
        {
            // Находим IndividualEntrepreneur по Id запроса
            var entity = await _IERepository.Select()
                .FirstOrDefaultAsync(ie => ie.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            _IERepository.Entry(entity).Reference(IE => IE.Founder).Load();

            // Находим учредителя, связанного с IndividualEntrepreneurId
            var founder = await _founderRepository.Select()
                .Include(f => f.IndividualEntrepreneur)
                .SingleOrDefaultAsync(f => f.Id == entity.FounderId, cancellationToken);

            //Начало транзации удаления ИП и связанной с ним поля сущности у учредители
            using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    //Если есть учредитель связанный с этим ИП, то это поле в таблице учредителей будет null
                    if (founder != null)
                        founder.IndividualEntrepreneur = null;

                    // Удаляем IndividualEntrepreneur
                    await _IERepository.Delete(entity.Id, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();                    
                }
                catch (System.Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            return Unit.Value;
        }
    }
}
