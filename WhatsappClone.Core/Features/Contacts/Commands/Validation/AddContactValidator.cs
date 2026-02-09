using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Contacts.Commands.Models;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Commands.Validation
{
    public class AddContactValidator : AbstractValidator<AddContactCommand>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IContactsService contactsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AddContactValidator(UserManager<AppUser> userManager, IContactsService contactsService, IHttpContextAccessor httpContextAccessor)
        {
            ApplyValidation();
            ApplyCustomValidation();
            this.userManager = userManager;
            this.contactsService = contactsService;
            this.httpContextAccessor = httpContextAccessor;
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
             .MustAsync(NotBeOwnPhoneNumber)
            .WithMessage("You cannot add yourself as a contact.")

            .MustAsync(PhoneNumberMustExistInSystem)
            .WithMessage("This person is not on WhatsApp.")

            .MustAsync(PhoneNumberMustNotBeInContactsList)
            .WithMessage("You have already added this contact.");


        }

        private async Task<bool> PhoneNumberMustNotBeInContactsList(string phoneNumber, CancellationToken cancellationToken)
        {
            var currentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return true;
            }

            var contactId = userManager.FindByPhoneNumber(phoneNumber)?.Id;

            if (contactId == null)
            {
                return true;
            }
            return !await contactsService.IsContactAdded(currentUserId, phoneNumber);

        }

        private async Task<bool> PhoneNumberMustExistInSystem(string phoneNumber, CancellationToken cancellationToken)
        {
            var contact = userManager.FindByPhoneNumber(phoneNumber);
            return contact != null;
        }

        private async Task<bool> NotBeOwnPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
        {
            var currentUserPhone = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone);
            if (currentUserPhone == null)
            {
                return true;

            }

            return currentUserPhone != phoneNumber;
        }


    }
}
