using FluentValidation;
using System;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    public class GetLegalEntityDetailsQueryValidator 
        : AbstractValidator<GetLegalEntityDetailsQuery>
    {
        public GetLegalEntityDetailsQueryValidator()
        {
            RuleFor(EL => EL.Id).NotEqual(Guid.Empty);
        }
    }
}
