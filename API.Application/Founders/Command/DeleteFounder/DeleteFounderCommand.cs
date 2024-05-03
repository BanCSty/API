using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommand : IRequest
    {
        public Guid FounderId { get; set; }
    }
}
