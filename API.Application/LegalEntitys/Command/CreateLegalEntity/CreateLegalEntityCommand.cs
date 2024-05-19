using MediatR;
using System.Collections.Generic;
namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommand : IRequest
    {
        public string INN { get; set; }

        public string Name { get; set; }

        public List<string> FounderINNs { get; set; }
    }
}
