using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommand : IRequest<Guid>
    {
        public long INN { get; set; }
        public string Name { get; set; }
        public List<Guid> FounderId { get; set; }
    }
}
