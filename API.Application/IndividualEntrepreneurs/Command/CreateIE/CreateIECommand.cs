using MediatR;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommand : IRequest<Guid>
    {
        public long INN { get; set; }
        public string Name { get; set; }
    }
}
