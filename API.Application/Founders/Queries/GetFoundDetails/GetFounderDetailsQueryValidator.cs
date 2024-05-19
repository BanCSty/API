using FluentValidation;
using System;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQueryValidator : AbstractValidator<GetFounderDetailsQuery>
    {
        public GetFounderDetailsQueryValidator()
        {
            RuleFor(founder => founder.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits."); ;
        }
    }
}
