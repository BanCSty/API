using MediatR;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommand : IRequest
    {

        public string INN { get; set; }

        public string Name { get; set; }

        public string FounderINN { get; set; }
    }
}
