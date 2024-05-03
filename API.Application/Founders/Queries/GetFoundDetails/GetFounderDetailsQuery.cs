using API.Domain;
using MediatR;
using System;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQuery : IRequest<Founder>
    {
        public Guid Id { get; set; }
    }
}
