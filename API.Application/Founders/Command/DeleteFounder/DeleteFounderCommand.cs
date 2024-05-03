using MediatR;
using System;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommand : IRequest
    {
        public Guid FounderId { get; set; }
    }
}
