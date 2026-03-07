using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Implementation;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Contacts.Commands
{
    public class AddContactCommandHandler : ResponseHandler,
        IRequestHandler<AddContactCommand, Response<string>>
    {
        private readonly SqlDBContext dBContext;
        private readonly PhoneNumberService phoneNumberService;
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;

        public AddContactCommandHandler(
            SqlDBContext dBContext,
            PhoneNumberService phoneNumberService,
            UserManager<User> userManager,  
            ICurrentUserService currentUserService
            )
        {
            this.dBContext = dBContext;
            this.phoneNumberService = phoneNumberService;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<string>> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            var (cleanedCountryCode, CleanedNationalNumber) = phoneNumberService.CleanPhoneNumber(request.CountryCode, request.PhoneNumber);
            var contactName = (request.FirstName is null && request.LastName is null) ? $"{cleanedCountryCode + CleanedNationalNumber}" : request.FullName.Trim();
            var currentUserId = currentUserService.UserId; 
            var contact = await userManager.FindByPhoneNumber(cleanedCountryCode, CleanedNationalNumber);
            if (contact == null)
            {
                return BadRequest<string>("This phone number is not on Whatsapp");
            }
            //check if contact already exists

            var contactFromDb = await dBContext.Contacts.FirstOrDefaultAsync(c => c.UserId == currentUserId && c.ContactUserId == contact.Id);

            if (contactFromDb is not null)
            {
                //edit contact name
                contactFromDb.ContactName = contactName;
                await dBContext.SaveChangesAsync();


                return Success<string>(contact.Id.ToString(), "Contact was added to your contacts");

            }

            var newContact = new Contact
            {
                ContactName = contactName,
                AddedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = currentUserId,
                ContactUserId = contact.Id,

            };
            await dBContext.AddAsync(newContact);
            await dBContext.SaveChangesAsync();

            return Success<string>(contact.Id.ToString(), "Contact was added to your contacts");
        }
    }
}
