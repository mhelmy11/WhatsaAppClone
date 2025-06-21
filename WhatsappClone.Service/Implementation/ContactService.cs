using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class ContactService : IContactsService
    {
        private readonly IUserContacts userContactsRepo;
        private readonly IChatService chatService;
        private readonly UserManager<AppUser> userManager;

        public ContactService(IUserContacts userContactsRepo, IChatService chatService, UserManager<AppUser> userManager)
        {
            this.userContactsRepo = userContactsRepo;
            this.chatService = chatService;
            this.userManager = userManager;
        }
        public async Task<UserContact> AddContactAsync(string userId, string contactId, string FName, string LName)
        {
            var contact = await userContactsRepo.AddAsync(new UserContact
            {
                UserId = userId,
                ContactId = contactId
            });


            var chat = await chatService.AddChatAsync(new Chat
            {
                ReceiverId = userId,
                SenderId = contactId,
                ChatName = $"{FName} {LName}",
            });
            return contact;
        }

        public async Task<UserContact> AddContactAsync(UserContact contact)
        {
            await userContactsRepo.AddAsync(contact);

            return contact;
        }

        public async Task DeleteContactAsync(string contactId, string userId)
        {
            await userContactsRepo.DeleteAsync(new UserContact
            {
                ContactId = contactId,
                UserId = userId
            });
        }

        public async Task EditContactAsync(UserContact contact)
        {
            await userContactsRepo.UpdateAsync(contact);
        }

        public List<UserContact> GetContacts(string currentUserId)
        {
            var contacts = userContactsRepo.GetContactsOrderedByAlpha(currentUserId);
            return contacts;
        }

        public async Task<bool> IsContactAdded(string userId, string phoneNumber)
        {
            var Contact = userManager.FindByPhoneNumber(phoneNumber);
            if (Contact == null)
            {
                return true;
            }

            return userContactsRepo.GetTableNoTracking().Any(x => x.UserId == userId && x.ContactId == Contact.Id);



        }
        public bool IsContactAddedByid(string userId, string contactId)
        {

            return userContactsRepo.GetTableNoTracking().Any(x => x.UserId == userId && x.ContactId == contactId);



        }
    }
}

