using FluentValidation;
using System;

namespace API.Application.LegalEntitys.Command.DeleteLegalEntity
{
    public class DeleteLegalEntityCommandValidator : AbstractValidator<DeleteLegalEntityCommand>
    {
        public DeleteLegalEntityCommandValidator()
        {
            RuleFor(deleteLECommand => deleteLECommand.Id).NotEqual(Guid.Empty);
        }
    }
}
