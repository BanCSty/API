using MediatR;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommand : IRequest
    {

        public string INN { get; set; }

        public string Name { get; set; }

        public string FounderINN { get; set; }
    }
}
