using FluentValidation;
using System;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQueryValidator : AbstractValidator<GetIEDetailsQuery>
    {
        public GetIEDetailsQueryValidator()
        {
            RuleFor(getIEDetailsQuery => getIEDetailsQuery.Id).NotEqual(Guid.Empty);
        }
    }
}
