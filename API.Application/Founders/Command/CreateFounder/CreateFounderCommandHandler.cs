using API.Application.Interfaces;
using API.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.CreateFounder
{
    public class CreateFounderCommandHandler
        : IRequestHandler<CreateFounderCommand, Guid>
    {
        private readonly IApiDbContext _dbContext;

        public CreateFounderCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateFounderCommand request, CancellationToken cancellationToken)
        {
            var founder = new Founder
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DateCreate = DateTime.Now,
                DateUpdate = null
            };

            await _dbContext.Founders.AddAsync(founder);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return founder.Id;
        }
    }
}
