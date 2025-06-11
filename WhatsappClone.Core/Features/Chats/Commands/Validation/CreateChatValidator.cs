using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Chats.Commands.Models;

namespace WhatsappClone.Core.Features.Chats.Commands.Validation
{
    public class CreateChatValidator : AbstractValidator<CreateChatCommand>
    {
        public CreateChatValidator()
        {

            ApplyValidations();


        }
        public void ApplyValidations()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Chat name is required.")
                .MaximumLength(100).WithMessage("Chat name cannot exceed 100 characters.");

        }
    }
}
