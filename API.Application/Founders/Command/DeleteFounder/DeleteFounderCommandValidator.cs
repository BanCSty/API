using FluentValidation;
using System;

namespace API.Application.Founders.Command.DeleteFounder
{
    public class DeleteFounderCommandValidator : AbstractValidator<DeleteFounderCommand>
    {
        public DeleteFounderCommandValidator()
        {
            RuleFor(deleteFounderCommand => deleteFounderCommand.FounderId).NotEqual(Guid.Empty);
        }
    }
}
