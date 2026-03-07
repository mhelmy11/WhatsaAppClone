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
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Commands.UnblockUser
{
    public class UnblockUserCommandHandler(SqlDBContext dBContext , ICurrentUserService currentUserService) : ResponseHandler, IRequestHandler< UnblockUserCommand , Response<string>>
    {
        private readonly ICurrentUserService currentUserService = currentUserService;

        public async Task<Response<string>> Handle(UnblockUserCommand request, CancellationToken ct)
        {
            var currentUserId = currentUserService.UserId;

            int deletedRows = await dBContext.BlockedUsers
                            .Where(c => c.UserId == currentUserId && c.BlockedUserId == long.Parse(request.BlockedUserId))
                            .ExecuteDeleteAsync(ct);

            if (deletedRows == 0)
            {
                return BadRequest<string>("user not found in your list.");
            }


            return Success<string>(null, "User is unblocked  successfully");
        }
    }
}
