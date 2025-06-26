using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Messages.Commands.Models;

namespace WhatsappClone.Core.Features.Messages.Commands.Validation
{
    public class SendGroupMessageValidator : AbstractValidator<SendGroupMessageCommand>
    {


        public SendGroupMessageValidator()
        {

            ApplyValidations();
            ApplyCustomValidations();

        }


        public void ApplyValidations()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Group ID is required.");

        }


        public void ApplyCustomValidations()
        {
            RuleFor(x => x.Content)
            .Must((command, content) =>
            {
                bool hasContent = !string.IsNullOrWhiteSpace(content);
                bool hasAttachments = command.Attachments != null && command.Attachments.Any();

                return hasContent || hasAttachments;
            })
            .WithMessage("A message must have either content or at least one attachment.");
        }
    }
}
