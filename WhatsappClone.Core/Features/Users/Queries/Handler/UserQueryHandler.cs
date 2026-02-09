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
using WhatsappClone.Core.Features.Users.Queries.Models;
using WhatsappClone.Core.Features.Users.Queries.Results;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Core.Features.Users.Queries.Handler
{
    public class UserQueryHandler : ResponseHandler, IRequestHandler<GetMeQuery, Response<GetMeQueryResult>>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public UserQueryHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<Response<GetMeQueryResult>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.FindByIdAsync(currentUserId);
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
