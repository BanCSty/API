using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQuery : IRequest<IEDetailsVm>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
