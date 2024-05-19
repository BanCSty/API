using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.LegalEntitys.Command.CreateLegalEntity
{
    public class CreateLegalEntityCommandValidator : AbstractValidator<CreateLegalEntityCommand>
    {
        public CreateLegalEntityCommandValidator()
        {
            RuleFor(createLECommand => createLECommand.INN)
                .NotEmpty()
                .Length(12)
                .Matches(@"^\d+$").WithMessage("INN must be exactly 12 digits.");

            RuleFor(createLECommand => createLECommand.Name).NotEmpty().MaximumLength(30);
        }
    }
}
