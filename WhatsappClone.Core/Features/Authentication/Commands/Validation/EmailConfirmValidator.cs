using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Authentication.Commands.Models;

namespace WhatsappClone.Core.Features.Authentication.Commands.Validation
{
    public class EmailConfirmValidator : AbstractValidator<EmailConfirmCommand>
    {

        public EmailConfirmValidator()
        {
            ApplyValidations();
            ApplyCustomValidations();
        }

        public void ApplyValidations()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Invalid email confirmation link.")
                .NotNull().WithMessage("Invalid email confirmation link.");



            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Invalid email confirmation link.")
                .NotNull().WithMessage("Invalid email confirmation link.");
        }

        public void ApplyCustomValidations()
        {
            // Add any custom validations here if needed
        }
    }
}
