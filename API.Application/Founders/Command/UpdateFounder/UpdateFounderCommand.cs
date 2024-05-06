using MediatR;
using System;
using System.Collections.Generic;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommand : IRequest
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<Guid>? LegalEntityIds { get; set; }
    }
}
