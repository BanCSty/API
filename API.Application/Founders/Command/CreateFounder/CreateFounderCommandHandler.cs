using API.DAL.Interfaces;
using API.Domain;
using API.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.CreateFounder
{
    public class CreateFounderCommandHandler
        : IRequestHandler<CreateFounderCommand>
    {
        private readonly IBaseRepository<Founder> _founderRepository;

        public CreateFounderCommandHandler(IBaseRepository<Founder> founderRepository)
        {
            _founderRepository = founderRepository;
        }

        public async Task<Unit> Handle(CreateFounderCommand request, CancellationToken cancellationToken)
        {

            var founderExist = await _founderRepository.Select()
                .FirstOrDefaultAsync(f => f.INN == (INN)request.INN, cancellationToken);

            //Если Founder INN уже существует и используется в другой сущности(отличной от изменяемой)
            //, то выбрасываем исключение.Для предотвращения данных учредителей с одинаковыми ИНН
            if (founderExist != null)
            {
                throw new ArgumentException($"INN: {request.INN} already used");
            }

            var founder = new Founder
                (
                    (INN)request.INN,
                    new FullName(
                        request.FirstName,
                        request.LastName,
                        request.MiddleName),
                    DateTime.Now
                );

            await _founderRepository.Create(founder, cancellationToken);

            return Unit.Value;
        }
    }
}
