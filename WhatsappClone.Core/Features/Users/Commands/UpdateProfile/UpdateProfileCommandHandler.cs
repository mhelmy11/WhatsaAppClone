using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : ResponseHandler, IRequestHandler<UpdateProfileCommand, Response<string>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<User> userManager;

        public UpdateProfileCommandHandler(ICurrentUserService currentUserService , UserManager<User> userManager)
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }
        public async Task<Response<string>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var currentUser = await userManager.FindByIdAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                return BadRequest<string>("User not found");
            }

            if (!string.IsNullOrEmpty(request.ProfilePic))
            {
                currentUser.ProfilePicUrl = request.ProfilePic;

            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                currentUser.Name = request.Name;

            }

            if (!string.IsNullOrEmpty(request.About))
            {
                currentUser.About = request.About;

            }

            await userManager.UpdateAsync(currentUser);

            return Success<string>(null, "Profile is updated successfully");

            

            
        }
    }
}
