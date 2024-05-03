using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommandHandler 
        : IRequestHandler<DeleteFounderCommand>
    {
        private readonly IApiDbContext _dbContext;

        public DeleteFounderCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteFounderCommand request, 
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Founders
                .FindAsync(new object[] { request.FounderId }, cancellationToken);

            if (entity == null || entity.Id != request.FounderId)
            {
                throw new NotFoundException(nameof(Founders), request.FounderId);
            }

            _dbContext.Founders.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //Пустой ответ(для тестирования)
            return Unit.Value;
        }
    }
}
