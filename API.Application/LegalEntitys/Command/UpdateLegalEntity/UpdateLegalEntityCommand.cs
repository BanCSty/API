using API.Domain;
using MediatR;
using System;
using System.Collections.Generic;

namespace API.Application.LegalEntitys.Command.UpdateLegalEntity
{
    public class UpdateLegalEntityCommand : IRequest
    {
        public Guid Id { get; set; }
        public long INN { get; set; }
        public string Name { get; set; }
        public List<Guid>? FounderIds { get; set; }
    }
}
