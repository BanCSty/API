using API.Application.Interfaces;
using API.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommandHandler
        : IRequestHandler<CreateLegalEntityCommand, Guid>
    {
        private readonly IApiDbContext _dbContext;

        public CreateLegalEntityCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateLegalEntityCommand request, CancellationToken cancellationToken)
        {
            var LE = new LegalEntity
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                Name = request.Name,
                DateCreate = DateTime.Now,
                DateUpdate = null,
                FounderId = request.FounderId
            };

            await _dbContext.LegalEntitys.AddAsync(LE);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return LE.Id;
        }
    }
}
