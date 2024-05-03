using FluentValidation;

namespace API.Application.Founders.Command.CreateFounder
{
    public class CreateFounderCommandValidator : AbstractValidator<CreateFounderCommand>
    {
        public CreateFounderCommandValidator()
        {
            RuleFor(createFounderCommand => createFounderCommand.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(createFounderCommand => createFounderCommand.LastName).NotEmpty().MaximumLength(20);
            RuleFor(createFounderCommand => createFounderCommand.MiddleName).NotEmpty().MaximumLength(20);
        }
    }
}
