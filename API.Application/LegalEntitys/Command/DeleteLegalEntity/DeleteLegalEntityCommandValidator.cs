using FluentValidation;
using System;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommandValidator : AbstractValidator<DeleteLegalEntityCommand>
    {
        public DeleteLegalEntityCommandValidator()
        {
            RuleFor(deleteLECommand => deleteLECommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");
        }
    }
}
