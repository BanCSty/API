using API.WebApi.Attributes;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommand : IRequest<Guid>
    {
        [Required]
        [INN12Digits]
        public string INN { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public Guid FounderId { get; set; }
    }
}
