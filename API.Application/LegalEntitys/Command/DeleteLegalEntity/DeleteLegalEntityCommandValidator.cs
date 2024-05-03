using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
