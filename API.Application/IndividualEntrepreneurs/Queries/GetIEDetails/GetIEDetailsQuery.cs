using API.Domain;
using MediatR;
using System;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQuery : IRequest<IndividualEntrepreneur>
    {
        public Guid Id { get; set; }
    }
}
