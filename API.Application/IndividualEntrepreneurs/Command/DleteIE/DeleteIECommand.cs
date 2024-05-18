using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommand : IRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
