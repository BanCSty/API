using FluentValidation;

namespace API.Application.Founders.Command.CreateFounder
{
    public class CreateFounderCommandValidator : AbstractValidator<CreateFounderCommand>
    {
        public CreateFounderCommandValidator()
        {
            RuleFor(createFounderCommand => createFounderCommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(createFounderCommand => createFounderCommand.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(createFounderCommand => createFounderCommand.LastName).NotEmpty().MaximumLength(20);
            RuleFor(createFounderCommand => createFounderCommand.MiddleName).NotEmpty().MaximumLength(20);
        }
    }
}
