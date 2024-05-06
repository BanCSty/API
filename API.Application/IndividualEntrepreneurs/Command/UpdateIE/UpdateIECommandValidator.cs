using FluentValidation;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.UpdateIE
{
    public class UpdateIECommandValidator
        : AbstractValidator<UpdateIECommand>
    {
        public UpdateIECommandValidator()
        {
            RuleFor(updateLECommand => updateLECommand.Id).NotEqual(Guid.Empty);
            RuleFor(updateLECommand => updateLECommand.INN).NotEmpty();
            RuleFor(updateLECommand => updateLECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
