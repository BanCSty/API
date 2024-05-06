using API.Domain;
using MediatR;
using System;
using System.Collections.Generic;


namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommand : IRequest<Guid>
    {
        public long INN { get; set; }
        public string Name { get; set; }
        public List<Guid> FounderIds { get; set; }
    }
}
