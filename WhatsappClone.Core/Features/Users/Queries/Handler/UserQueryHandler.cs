using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Core.Features.Users.Queries.Models;
using WhatsappClone.Core.Features.Users.Queries.Results;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Core.Features.Users.Queries.Handler
{
    public class UserQueryHandler : ResponseHandler, IRequestHandler<GetMeQuery, Response<GetMeQueryResult>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public UserQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<Response<GetMeQueryResult>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return BadRequest<GetMeQueryResult>("User Not Found");
            }

            // Map the user to the GetMeQueryResult
            var result = mapper.Map<GetMeQueryResult>(user);
            // Return the result wrapped in a Response object

            return Success(result);
        }
    }
}
