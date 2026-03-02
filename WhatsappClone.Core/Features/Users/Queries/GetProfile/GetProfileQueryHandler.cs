using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Queries
{
    public class GetProfileQueryHandler : ResponseHandler, IRequestHandler<GetProfileQuery, Response<GetProfileResult>>
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PhoneNumberService phoneNumberService;
        private readonly SqlDBContext dBContext;
        private readonly IMongoDBFactory mongoDB;

        public GetProfileQueryHandler(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            SqlDBContext dBContext,
            IMongoDBFactory mongoDB

            )
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
            this.mongoDB = mongoDB;
        }
        public async Task<Response<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);

            if (currentUser.Id.ToString() == request.UserId)
            {
                return Success(new GetProfileResult() { Name = "me(You)" });
            }
            var targetUser = await userManager.FindByIdAsync(request.UserId);
            var presenceCollection = mongoDB.GetCollection<Presence>();
            var filter = Builders<Presence>.Filter.Eq(f => f.UserId, targetUser.Id);
            var projection = Builders<Presence>.Projection.Expression(p => p.CanViewProfilePicMyContactsExcept);
            var targetUserProfileExceptionList = await presenceCollection.Find(filter).Project(projection).FirstOrDefaultAsync() ?? new();
            if (targetUser == null)
            {
                return BadRequest<GetProfileResult>("User is not found");
            }

            var getMyProfileResult = new GetProfileResult();
            getMyProfileResult.CountryCode = targetUser.CountryCode;
            getMyProfileResult.PhoneNumber = targetUser.PhoneNumber!;
            getMyProfileResult.About = targetUser.About;



            //check if targetUser exists as contact for currentUser
            var isTargetContactForCurrent = await dBContext.CheckContactExistsByIdAsync(currentUser.Id.ToString(), targetUser.Id.ToString(), cancellationToken);
            var isCurrentContactForTarget = await dBContext.CheckContactExistsByIdAsync(targetUser.Id.ToString(), currentUser.Id.ToString(), cancellationToken);
            if (!isTargetContactForCurrent)
            {
                getMyProfileResult.IsContact = false;
                getMyProfileResult.Name = targetUser.UserName!;

            }
            getMyProfileResult.Name = (await dBContext.Contacts.FirstOrDefaultAsync(c=> c.UserId == currentUser.Id && c.ContactUserId == targetUser.Id)).ContactName;
            //check profile pic privacy for targetUser either contact or not
            // show it only if privacy is (everyone | Contacts)
            var profilePicPrivacy = targetUser.WhoCanViewProfilePic;
            getMyProfileResult.ProfilePic = profilePicPrivacy switch
            {
                "Everyone" => targetUser.ProfilePicUrl,
                "Contacts" => isCurrentContactForTarget ? profilePicPrivacy : "default",
                "MyContactsExcept" => isCurrentContactForTarget && !targetUserProfileExceptionList.Any(c => c == currentUser.Id) ? targetUser.ProfilePicUrl : "default"  ,
                _ => "default"
            };

            return Success(getMyProfileResult);




        }
    }
}
