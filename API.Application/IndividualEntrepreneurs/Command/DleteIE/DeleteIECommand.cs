using MediatR;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommand : IRequest
    {
        public string INN { get; set; }
    }
}
