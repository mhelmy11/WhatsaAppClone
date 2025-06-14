using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Authentication.Commands.Models;

namespace WhatsappClone.Core.Features.Authentication.Commands.Validation
{


    public class LoginValidator : AbstractValidator<LoginCommand>
    {

        public LoginValidator()
        {
            ApplyValidations();
            ApplyCustomValidations();

        }


        public void ApplyValidations()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");




            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");




        }

        public void ApplyCustomValidations()
        {
            // Add any custom validations here if needed
        }

    }
}
