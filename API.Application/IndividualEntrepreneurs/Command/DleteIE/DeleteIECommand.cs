using MediatR;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
