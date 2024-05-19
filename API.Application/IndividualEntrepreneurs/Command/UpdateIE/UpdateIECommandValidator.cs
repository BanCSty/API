using FluentValidation;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommandValidator
        : AbstractValidator<UpdateIECommand>
    {
        public UpdateIECommandValidator()
        {
            RuleFor(updateLECommand => updateLECommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(updateLECommand => updateLECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
