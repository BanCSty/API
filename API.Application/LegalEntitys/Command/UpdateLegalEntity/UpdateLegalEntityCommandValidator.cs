using FluentValidation;
using System;

namespace API.Application.LegalEntitys.Command.UpdateLegalEntity
{
    public class UpdateLegalEntityCommandValidator 
        : AbstractValidator<UpdateLegalEntityCommand>
    {
        public UpdateLegalEntityCommandValidator()
        {
            RuleFor(updateLECommand => updateLECommand.Id).NotEqual(Guid.Empty);
            RuleFor(updateLECommand => updateLECommand.INN).NotEmpty();
            RuleFor(updateLECommand => updateLECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
