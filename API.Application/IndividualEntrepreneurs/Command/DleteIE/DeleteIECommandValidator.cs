using FluentValidation;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.DeleteIE
{
    public class DeleteIECommandValidator : AbstractValidator<DeleteIECommand>
    {
        public DeleteIECommandValidator()
        {
            RuleFor(deleteIECommand => deleteIECommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");
        }
    }
}
