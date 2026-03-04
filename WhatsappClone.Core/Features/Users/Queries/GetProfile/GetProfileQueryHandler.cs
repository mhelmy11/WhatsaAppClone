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

        public GetProfileQueryHandler(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            SqlDBContext dBContext

            )
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
        }
        public async Task<Response<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {

            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);
            if (currentUser.Id == long.Parse(request.UserId))
            {
                return Success(new GetProfileResult() { Name = "me(You)", ProfilePic = currentUser.ProfilePicUrl });
            }

            long targetUserId = long.Parse(request.UserId);

            var targetData = await dBContext.Users
            .AsNoTracking()
            .Where(u => u.Id == targetUserId)
            .Select(u => new
            {
                u.CountryCode,
                u.PhoneNumber,
                u.About,
                u.UserName,
                u.ProfilePicUrl,
                Privacy = u.PrivacySettings.ProfilePicPrivacy,
                AmIInHisContacts = dBContext.Contacts
                    .Any(c => c.UserId == targetUserId && c.ContactUserId == currentUser.Id),
                MySavedContact = dBContext.Contacts
                    .FirstOrDefault(c => c.UserId == currentUser.Id && c.ContactUserId == targetUserId),
                AmIExcludedFromPic = u.PrivacySettings.PrivacyExceptions
                    .Any(e => e.IsExcludedFromProfilePic &&e.ExcludedContactId == currentUser.Id)
            })
            .FirstOrDefaultAsync(cancellationToken);

            if (targetData == null)
            {
                return BadRequest<GetProfileResult>("User not found.");
            }
            var result = new GetProfileResult
            {
                CountryCode = targetData.CountryCode,
                PhoneNumber = targetData.PhoneNumber!,
                About = targetData.About,
                Name = targetData.MySavedContact != null ? targetData.MySavedContact.ContactName : targetData.UserName!,
                IsContact = targetData.MySavedContact != null
            };

            result.ProfilePic = targetData.Privacy switch
            {
                PrivacyLevel.Everyone => targetData.ProfilePicUrl,

                PrivacyLevel.MyContacts when targetData.AmIInHisContacts => targetData.ProfilePicUrl,

                PrivacyLevel.MyContactsExcept when targetData.AmIInHisContacts && !targetData.AmIExcludedFromPic => targetData.ProfilePicUrl,

                _ => "default_avatar_url"
            };

            return Success(result);







            //var currentUser = await userManager.GetCurrentUser(httpContextAccessor);

            //if (currentUser.Id == long.Parse(request.UserId))
            //{
            //    return Success(new GetProfileResult() { Name = "me(You)" , ProfilePic = currentUser.ProfilePicUrl });
            //}
            //var targetUser = await dBContext.Users.Include(c => c.PrivacySettings).ThenInclude(p => p.PrivacyExceptions).FirstOrDefaultAsync(u => u.Id == long.Parse(request.UserId)); 
            //if (targetUser == null)
            //{
            //    return BadRequest<GetProfileResult>("User is not found");
            //}

            //var getMyProfileResult = new GetProfileResult();
            //getMyProfileResult.CountryCode = targetUser.CountryCode;
            //getMyProfileResult.PhoneNumber = targetUser.PhoneNumber!;
            //getMyProfileResult.About = targetUser.About;



            ////check if targetUser exists as contact for currentUser
            //var isTargetContactForCurrent = await dBContext.CheckContactExistsByIdAsync(currentUser.Id.ToString(), targetUser.Id.ToString(), cancellationToken);
            //var isCurrentContactForTarget = await dBContext.CheckContactExistsByIdAsync(targetUser.Id.ToString(), currentUser.Id.ToString(), cancellationToken);
            //if (!isTargetContactForCurrent)
            //{
            //    getMyProfileResult.IsContact = false;
            //    getMyProfileResult.Name = targetUser.UserName!; // DisplayName

            //}
            //getMyProfileResult.Name = (await dBContext.Contacts.FirstOrDefaultAsync(c=> c.UserId == currentUser.Id && c.ContactUserId == targetUser.Id)).ContactName;
            ////check profile pic privacy for targetUser either contact or not
            //// show it only if privacy is (everyone | Contacts)
            //var profilePicPrivacy = targetUser.PrivacySettings.ProfilePicPrivacy;
            //getMyProfileResult.ProfilePic = profilePicPrivacy switch
            //{
            //    PrivacyLevel.Everyone => targetUser.ProfilePicUrl,
            //    PrivacyLevel.MyContacts => isCurrentContactForTarget ? targetUser.ProfilePicUrl : "default",
            //   PrivacyLevel.MyContactsExcept => isCurrentContactForTarget && !targetUser.PrivacySettings.PrivacyExceptions.Any(p => long.Parse(p.ExcludedContactId) == currentUser.Id) ? targetUser.ProfilePicUrl : "default"  ,
            //    _ => "default"
            //};

            //return Success(getMyProfileResult);




        }
    }
}
