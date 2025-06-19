using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Contacts.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Commands.Validation
{
    public class AddContactValidator : AbstractValidator<AddContactCommand>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IContactsService contactsService;

        public AddContactValidator(UserManager<AppUser> userManager, IContactsService contactsService)
        {
            ApplyValidation();
            ApplyCustomValidation();
            this.userManager = userManager;
            this.contactsService = contactsService;
        }


        public void ApplyValidation()
        {
            RuleFor(x => x.FName)
                .NotEmpty()
                .WithMessage("First name is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .Matches(@"^\+\d{1,3}\d{9,15}$")
                .WithMessage("Phone number must be in international format (e.g., +1234567890).");

        }


        public void ApplyCustomValidation()
        {

            RuleFor(x => x.PhoneNumber)
                .MustAsync(async (phoneNumber, cancellation) =>
                {
                    var existingUser = userManager.FindByPhoneNumber(phoneNumber);
                    return existingUser != null;

                }).WithMessage("This phone number is not on Whatsapp. Please invite them to join WhatsApp.")
                ;


            //ensure that the contact is not already added
            RuleFor(x => x)
                .MustAsync(async (user, cancellation) =>

                {
                    var isContactAdded = await contactsService.IsContactAdded(user.userId, user.PhoneNumber);
                    return !isContactAdded;


                }).WithMessage("This contact is already added.")
                ;

            //ensure that not the same user is being added as a contact
            RuleFor(x => x)
                .MustAsync(async (user, cancellation) =>
                {
                    var existingUser = await userManager.FindByIdAsync(user.userId);
                    return existingUser != null && existingUser.PhoneNumber != user.PhoneNumber;

                }).WithMessage("You cannot add yourself as a contact.")
                ;
        }


    }
}
