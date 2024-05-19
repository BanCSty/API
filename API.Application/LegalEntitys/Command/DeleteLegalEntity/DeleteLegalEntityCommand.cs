using MediatR;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommand : IRequest
    {
        public string INN { get; set; }
    }
}
