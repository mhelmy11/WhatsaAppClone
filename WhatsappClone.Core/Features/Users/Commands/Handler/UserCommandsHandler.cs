using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Commands.Models;
using WhatsappClone.Data.Models;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Commands.Handler
{
    public class UserCommandsHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public UserCommandsHandler(IMapper mapper, UserManager<AppUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;


        }
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = userManager.FindByPhoneNumber(request.PhoneNumber);

            if (user != null) return BadRequest<string>("Phone number is already in use.");
            user = await userManager.FindByNameAsync(request.UserName);
            if (user != null) return BadRequest<string>("Username is already in use.");


            var newUser = mapper.Map<AppUser>(request);

            var Result = await userManager.CreateAsync(newUser, request.Password);

            if (!Result.Succeeded)
            {
                var error = Result.Errors.Select(x => x.Description).FirstOrDefault();
                return BadRequest<string>(error);
            }

            return Success<string>("User Created Successfully. Please Login Now.");






        }
    }
}
