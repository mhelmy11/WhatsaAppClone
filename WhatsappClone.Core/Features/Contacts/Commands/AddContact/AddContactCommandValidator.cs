using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Features.Contacts.Commands.AddContact
{
    public class AddContactCommandValidator : AbstractValidator<AddContactCommand>
    {
        public AddContactCommandValidator() 
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number must not be empty")
                .NotNull().WithMessage("Phone Numeber must not be null");

            RuleFor(x => x.CountryCode).NotEmpty().WithMessage("Country Code must not be empty")
                .NotNull().WithMessage("Country Code must not be null");



        }
    }
}
