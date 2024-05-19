using API.WebApi.Attributes;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommand : IRequest<Guid>
    {
        [Required]
        [INN12Digits]
        public string INN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public List<Guid> FounderIds { get; set; }
    }
}
