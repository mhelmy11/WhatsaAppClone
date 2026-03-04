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
using WhatsappClone.Service.Helpers;
namespace WhatsappClone.Core.Features.Contacts.Commands
{
    public class DeleteContactCommandHandler : ResponseHandler, IRequestHandler<DeleteContactCommand, Response<string>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SqlDBContext dBContext;
        private readonly UserManager<User> userManager;

        public DeleteContactCommandHandler(
            IHttpContextAccessor httpContextAccessor,
            SqlDBContext dBContext,
            UserManager<User> userManager

            ) 
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
            this.userManager = userManager;
        }
        public async Task<Response<string>> Handle(DeleteContactCommand request, CancellationToken ct)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);

            int deletedRows = await dBContext.Contacts
                            .Where(c => c.UserId == currentUser.Id && c.ContactUserId == long.Parse(request.ContactId))
                            .ExecuteDeleteAsync(ct);

            if (deletedRows == 0)
            {
                return BadRequest<string>("Contact not found in your list.");
            }


            return Success<string>(null, "Contact is removed  successfully");
        }
    }
}
