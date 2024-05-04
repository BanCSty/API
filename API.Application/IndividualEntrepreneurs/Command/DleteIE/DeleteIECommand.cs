using MediatR;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class DeleteIECommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
