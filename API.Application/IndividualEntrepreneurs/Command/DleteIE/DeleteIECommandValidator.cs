using FluentValidation;
using System;

namespace API.Application.IndividualEntrepreneurs.Command.CreateIE
{
    public class DeleteIECommandValidator : AbstractValidator<DeleteIECommand>
    {
        public DeleteIECommandValidator()
        {
            RuleFor(DeleteIECommand => DeleteIECommand.Id).NotEqual(Guid.Empty);
        }
    }
}
