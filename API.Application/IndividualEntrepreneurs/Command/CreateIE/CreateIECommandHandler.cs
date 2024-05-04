using API.Application.Interfaces;
using API.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommandHandler
        : IRequestHandler<CreateIECommand, Guid>
    {
        private readonly IApiDbContext _dbContext;

        public CreateIECommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateIECommand request, CancellationToken cancellationToken)
        {
            var IE = new IndividualEntrepreneur
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.Now,
                DateUpdate = null,
            };

            await _dbContext.IndividualEntrepreneurs.AddAsync(IE);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return IE.Id;
        }
    }
}
