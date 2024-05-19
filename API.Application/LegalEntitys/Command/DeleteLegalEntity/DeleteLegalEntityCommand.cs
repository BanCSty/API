using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
