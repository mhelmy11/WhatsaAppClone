using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Features.Users.Commands.Validation
{
    public class ForgetPasswordValidation : AbstractValidator<ForgetPasswordCommand>
    {
        private readonly UserManager<AppUser> userManager;

        public ForgetPasswordValidation(UserManager<AppUser> userManager)
        {
            ApplyValidations();
            ApplyCustomValidations();
            this.userManager = userManager;
        }


        public void ApplyValidations()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }

        public void ApplyCustomValidations()
        {
            RuleFor(x => x.Email).MustAsync(async (email, cancellation) =>
            {
                var emailExists = await userManager.FindByEmailAsync(email);
                return emailExists != null;
            }).WithMessage("If an account with this email exists, a password reset link has been sent.");
        }
    }
}
