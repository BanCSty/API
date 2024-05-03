using API.Application.Common.Exceptions;
using API.Application.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommandHandler
        : IRequestHandler<UpdateFounderCommand>
    {
        private readonly IApiDbContext _dbContext;

        public UpdateFounderCommandHandler(IApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateFounderCommand request, 
            CancellationToken cancellationToken)
        {
            var entity = 
                await _dbContext.Founders.FirstOrDefaultAsync
                (founder => founder.Id == request.FounderId, cancellationToken);

            if (entity == null || entity.Id != request.FounderId)
            {
                throw new NotFoundException(nameof(Founder), request.FounderId);
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.MiddleName = request.MiddleName;
            entity.DateUpdate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
