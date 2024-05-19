using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommand : IRequest
    {
        [Required]
        public Guid FounderId { get; set; }
    }
}
