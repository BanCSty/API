using API.DAL.Interfaces;
using API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.CreateFounder
{
    public class CreateFounderCommandHandler
        : IRequestHandler<CreateFounderCommand, Guid>
    {
        private readonly IBaseRepository<Founder> _founderRepository;

        public CreateFounderCommandHandler(IBaseRepository<Founder> founderRepository)
        {
            _founderRepository = founderRepository;
        }

        public async Task<Guid> Handle(CreateFounderCommand request, CancellationToken cancellationToken)
        {

            var founderExist = await _founderRepository.Select().FirstOrDefaultAsync(f => f.INN == request.INN, cancellationToken);

            //Если Founder INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founderExist != null)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            var founder = new Founder
            {
                Id = Guid.NewGuid(),
                INN = request.INN,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DateCreate = DateTime.Now,
                DateUpdate = null
            };

            await _founderRepository.Create(founder, cancellationToken);

            return founder.Id;
        }
    }
}
