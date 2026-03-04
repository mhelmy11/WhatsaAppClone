using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Helpers;
using Microsoft.EntityFrameworkCore;

namespace WhatsappClone.Core.Features.Users.Commands.UnblockUser
{
    public class UnblockUserCommandHandler(UserManager<User> userManager , SqlDBContext dBContext , IHttpContextAccessor httpContextAccessor) : ResponseHandler, IRequestHandler< UnblockUserCommand , Response<string>>
    {
        public async Task<Response<string>> Handle(UnblockUserCommand request, CancellationToken ct)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);

            int deletedRows = await dBContext.BlockedUsers
                            .Where(c => c.UserId == currentUser.Id && c.BlockedUserId == long.Parse(request.BlockedUserId))
                            .ExecuteDeleteAsync(ct);

            if (deletedRows == 0)
            {
                return BadRequest<string>("user not found in your list.");
            }


            return Success<string>(null, "User is unblocked  successfully");
        }
    }
}
