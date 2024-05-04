using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class DeleteIECommandHandler
        : IRequestHandler<DeleteIECommand>
    {
        private readonly IApiDbContext _dbContext;

        public DeleteIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteIECommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.IndividualEntrepreneurs
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null || entity.Id != request.Id)
            {
                throw new NotFoundException(nameof(IndividualEntrepreneur), request.Id);
            }

            _dbContext.IndividualEntrepreneurs.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //Пустой ответ(для тестирования)
            return Unit.Value;
        }
    }
}
