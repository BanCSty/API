using MediatR;
using System;
namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommand : IRequest
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public Guid FounderId { get; set; }
    }
}
