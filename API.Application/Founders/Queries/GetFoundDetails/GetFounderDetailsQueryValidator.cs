using FluentValidation;
using System;

namespace API.Application.Founders.Queries.GetFoundDetails
{
    public class GetFounderDetailsQueryValidator : AbstractValidator<GetFounderDetailsQuery>
    {
        public GetFounderDetailsQueryValidator()
        {
            RuleFor(founder => founder.Id).NotEqual(Guid.Empty);
        }
    }
}
