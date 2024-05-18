using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{

    public class GetLegalEntityDetailsQuery : IRequest<LegalEntityDetailsVm>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
