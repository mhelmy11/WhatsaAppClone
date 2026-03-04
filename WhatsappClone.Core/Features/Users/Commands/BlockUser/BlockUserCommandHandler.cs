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
using EFCore.BulkExtensions;

namespace WhatsappClone.Core.Features.Users.Commands.BlockUser
{
    public class BlockUserCommandHandler(UserManager<User> userManager , SqlDBContext dBContext , IHttpContextAccessor httpContextAccessor) : ResponseHandler, IRequestHandler< BlockUserCommand , Response<string>>
    {
        public async Task<Response<string>> Handle(BlockUserCommand request, CancellationToken ct)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);

            await dBContext.BulkInsertOrUpdateAsync(new List<BlockedUser> { new BlockedUser
            {
                BlockedUserId = long.Parse(request.BlockedUserId),
                UserId = currentUser.Id,
                BlockedAt = DateTime.UtcNow,
            } });

            return Success<string>(null, "User is blocked successfully");
        }
    }
}
