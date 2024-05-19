using MediatR;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommand : IRequest
    {
        public string INN { get; set; }
    }
}
