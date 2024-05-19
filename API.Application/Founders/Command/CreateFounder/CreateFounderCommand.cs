using API.WebApi.Attributes;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.Founders.Command.CreateFounder
{
    //Содержит информацию, о том, что необходимо для создания учредителя
    public class CreateFounderCommand : IRequest<Guid>
    {
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
    }
}
