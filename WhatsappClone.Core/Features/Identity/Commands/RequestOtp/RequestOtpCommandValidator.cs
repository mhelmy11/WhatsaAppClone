using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Identity.Commands
{
    public class RequestOtpCommandValidator : AbstractValidator<RequestOtpCommand>
    {
        public RequestOtpCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").NotNull().WithMessage("Email is required").EmailAddress(EmailValidationMode.AspNetCoreCompatible);
        }
    }
}
