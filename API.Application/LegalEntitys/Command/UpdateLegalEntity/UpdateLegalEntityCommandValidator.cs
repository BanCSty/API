﻿using FluentValidation;

namespace API.Application.LegalEntitys.Command.UpdateLegalEntity
{
    public class UpdateLegalEntityCommandValidator 
        : AbstractValidator<UpdateLegalEntityCommand>
    {
        public UpdateLegalEntityCommandValidator()
        {
            RuleFor(updateLECommand => updateLECommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(updateLECommand => updateLECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
