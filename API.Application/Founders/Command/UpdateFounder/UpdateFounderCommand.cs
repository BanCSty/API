using MediatR;
using System;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommand : IRequest
    {
        public Guid FounderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
