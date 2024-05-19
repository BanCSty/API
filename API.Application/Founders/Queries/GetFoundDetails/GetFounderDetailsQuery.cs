using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQuery : IRequest<FounderDetailsVm>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
