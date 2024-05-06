using API.Domain;
using MediatR;
using System;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQuery : IRequest<FounderDetailsVm>
    {
        public Guid Id { get; set; }
    }
}
