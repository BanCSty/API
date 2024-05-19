using FluentValidation;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class CreateIECommandValidator : AbstractValidator<CreateIECommand>
    {
        public CreateIECommandValidator()
        {
            RuleFor(createIECommand => createIECommand.INN).NotEmpty();
            RuleFor(createIECommand => createIECommand.FounderId).NotEmpty();
            RuleFor(createIECommand => createIECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
