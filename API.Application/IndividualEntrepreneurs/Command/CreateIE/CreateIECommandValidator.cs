using FluentValidation;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommandValidator : AbstractValidator<CreateIECommand>
    {
        public CreateIECommandValidator()
        {
            RuleFor(createIECommand => createIECommand.INN).NotEmpty()
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(createIECommand => createIECommand.FounderINN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(createIECommand => createIECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
