using FluentValidation;
using System;

namespace API.Application.Founders.Command.UpdateFounder
{
    public class UpdateFounderCommandValidator : AbstractValidator<UpdateFounderCommand>
    {
        public UpdateFounderCommandValidator()
        {
            RuleFor(updateFounderCommand => updateFounderCommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(updateFounderCommand => updateFounderCommand.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(updateFounderCommand => updateFounderCommand.LastName).NotEmpty().MaximumLength(20);
            RuleFor(updateFounderCommand => updateFounderCommand.MiddleName).NotEmpty().MaximumLength(20);
        }      
    }
}
