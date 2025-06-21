using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Contacts.Queries.Models;
using WhatsappClone.Core.Features.Contacts.Queries.Results;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Contacts.Queries.Handler
{
    public class ContactQueryHandler : ResponseHandler, IRequestHandler<GetContactsQuery, Response<List<GetContactsQueryResult>>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IContactsService contactsService;

        public ContactQueryHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, IContactsService contactsService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.contactsService = contactsService;
        }
        public async Task<Response<List<GetContactsQueryResult>>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var CurrentUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ContactList = contactsService.GetContacts(CurrentUserId);

            if (ContactList == null)
            {
                return BadRequest<List<GetContactsQueryResult>>("No Contacts Found");
            }


            var contactList = contactsService.GetContacts(CurrentUserId);
            //Map the contact list to the GetContactsQueryResult
            var result = mapper.Map<List<GetContactsQueryResult>>(contactList);

            // Return the result wrapped in a Response object
            return Success(result, "Contacts Retrieved successfully.", result.Count());



        }
    }
}
