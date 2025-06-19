using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Contacts.Commands.Models;
using WhatsappClone.Core.Features.Contacts.Commands.Results;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Commands.Handler
{
    public class ContactCommandsHandler : ResponseHandler, IRequestHandler<AddContactCommand, Response<AddContactResult>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IChatService chatService;

        public ContactCommandsHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager, IChatService chatService, IContactsService contactsService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
            this.chatService = chatService;
            ContactsService = contactsService;
        }

        public IContactsService ContactsService { get; }

        public async Task<Response<AddContactResult>> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {


            request.userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var contact = userManager.FindByPhoneNumber(request.PhoneNumber);
            request.contactId = contact.Id;

            var addedContact = await ContactsService.AddContactAsync(request.userId, request.contactId, request.FName, request.LName);

            return Success(new AddContactResult
            {
                Id = addedContact.ContactId,
                PhoneNumber = contact.PhoneNumber,
                ChatName = $"{request.FName} {request.LName}"
            });

        }
    }
}
