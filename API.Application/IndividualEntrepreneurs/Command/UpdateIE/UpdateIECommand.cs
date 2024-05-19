using API.WebApi.Attributes;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommand : IRequest
    {
        [Required]
        public Guid Id { get; set; }

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
