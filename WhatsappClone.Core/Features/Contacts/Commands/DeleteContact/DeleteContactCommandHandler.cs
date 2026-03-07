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
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Helpers;
namespace WhatsappClone.Core.Features.Contacts.Commands
{
    public class DeleteContactCommandHandler : ResponseHandler, IRequestHandler<DeleteContactCommand, Response<string>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;

        public DeleteContactCommandHandler(
            SqlDBContext dBContext,
            ICurrentUserService currentUserService
            ) 
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<string>> Handle(DeleteContactCommand request, CancellationToken ct)
        {
            var currentUserId =currentUserService.UserId;

            int deletedRows = await dBContext.Contacts
                            .Where(c => c.UserId == currentUserId && c.ContactUserId == long.Parse(request.ContactId))
                            .ExecuteDeleteAsync(ct);

            if (deletedRows == 0)
            {
                return BadRequest<string>("Contact not found in your list.");
            }


            return Success<string>(null, "Contact is removed  successfully");
        }
    }
}
