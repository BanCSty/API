using API.WebApi.Attributes;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommand : IRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [INN12Digits]
        public string INN { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string MiddleName { get; set; }

        public Guid? IndividualEntrepreneurId { get; set; } = Guid.Empty;
        public List<Guid>? LegalEntityIds { get; set; } = new List<Guid>();
    }
}
