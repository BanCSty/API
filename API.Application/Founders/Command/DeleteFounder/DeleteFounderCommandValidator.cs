using FluentValidation;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommandValidator : AbstractValidator<DeleteFounderCommand>
    {
        public DeleteFounderCommandValidator()
        {
            RuleFor(deleteFounderCommand => deleteFounderCommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");
        }
    }
}
