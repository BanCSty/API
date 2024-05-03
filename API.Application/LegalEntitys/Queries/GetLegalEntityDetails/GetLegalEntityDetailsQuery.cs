using MediatR;
using System;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{

    public class GetLegalEntityDetailsQuery : IRequest<LegalEntityDetailsVm>
    {
        public Guid Id { get; set; }
    }
}
