using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Features.Users.Commands.Validation
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly UserManager<AppUser> userManager;

        public ResetPasswordValidator(UserManager<AppUser> userManager)
        {
            ApplyValidations();
            ApplyCustomValidations();
            this.userManager = userManager;
        }


        public void ApplyValidations()
        {
            RuleFor(x => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(x.Email)))
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .NotNull().WithMessage("Email cannot be null.");



            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Password and Confirm Password must match.");

            RuleFor(x => x.ResetToken)
                .NotEmpty().WithMessage("Reset token is required.")
                .NotNull().WithMessage("Reset token cannot be null.");

        }

        public void ApplyCustomValidations()
        {

            RuleFor(x => x.Email).MustAsync(async (email, cancellation) =>
            {
                var decodedEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(email));
                var emailExists = await userManager.FindByEmailAsync(decodedEmail);
                return emailExists != null;
            }).WithMessage("Password reset successful.");
        }
    }
}
