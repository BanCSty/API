using FluentValidation;

namespace API.Application.LegalEntitys.Queries.GetLegalEntityDetails
{
    public class GetLegalEntityDetailsQueryValidator 
        : AbstractValidator<GetLegalEntityDetailsQuery>
    {
        public GetLegalEntityDetailsQueryValidator()
        {
            RuleFor(LE => LE.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");
        }
    }
}
