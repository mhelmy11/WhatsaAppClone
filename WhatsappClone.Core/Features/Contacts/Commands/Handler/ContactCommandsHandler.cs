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
                                                         , IRequestHandler<EditContactCommand, Response<string>>
                                                         , IRequestHandler<DeleteContactCommand, Response<string>>
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


            var CurrentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var contact = userManager.FindByPhoneNumber(request.PhoneNumber);
            var ContactId = contact.Id;



            var addedContact = await ContactsService.AddContactAsync(new UserContact { ContactId = ContactId, UserId = CurrentUserId, FName = request.FName, LNAme = request.LName, FullName = $"{request.FName} {request.LName}".Trim() });

            return Success(new AddContactResult
            {
                Id = addedContact.ContactId,
                PhoneNumber = contact.PhoneNumber,
                ChatName = $"{request.FName} {request.LName}"
            }, "Contact Added. Chat now");

        }

        public async Task<Response<string>> Handle(EditContactCommand request, CancellationToken cancellationToken)
        {
            var CurrentUserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //map edit command to user contact model
            var contact = mapper.Map<UserContact>(request);

            contact.UserId = CurrentUserId!;

            //edit contact
            await ContactsService.EditContactAsync(contact);

            return Success<string>(contact.ContactId, "Edited Successfully");



        }

        public async Task<Response<string>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var CurrentUserId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = ContactsService.IsContactAddedByid(request.ContactId, CurrentUserId!);
            if (!result)
            {
                return BadRequest<string>("Contact not found");
            }
            await ContactsService.DeleteContactAsync(request.ContactId, CurrentUserId!);


            return Success<string>(request.ContactId, "Deleted Successfully");

        }
    }
}
