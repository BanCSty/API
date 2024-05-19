using FluentValidation;
using System;

namespace API.Application.IndividualEntrepreneurs.Queries.GetIEDetails
{
    public class GetIEDetailsQueryValidator : AbstractValidator<GetIEDetailsQuery>
    {
        public GetIEDetailsQueryValidator()
        {
            RuleFor(getIEDetailsQuery => getIEDetailsQuery.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");
        }
    }
}
