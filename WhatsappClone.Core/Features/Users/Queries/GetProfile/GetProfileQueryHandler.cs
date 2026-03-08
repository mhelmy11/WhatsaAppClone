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
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Implementation;

namespace WhatsappClone.Core.Features.Users.Queries
{
    public class GetProfileQueryHandler : ResponseHandler, IRequestHandler<GetProfileQuery, Response<GetProfileResult>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;

        public GetProfileQueryHandler(
            SqlDBContext dBContext,
            ICurrentUserService currentUserService
            )
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {

            var currentUserId = currentUserService.UserId;
            var currentUser = await dBContext.Users.Where(u => u.Id == currentUserId).Select(u => new{ u.ProfilePicUrl })
                .FirstOrDefaultAsync(cancellationToken);

            if (currentUserId == long.Parse(request.UserId))
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
                    .Any(c => c.UserId == targetUserId && c.ContactUserId == currentUserId),
                MySavedContact = dBContext.Contacts
                    .FirstOrDefault(c => c.UserId == currentUserId && c.ContactUserId == targetUserId),
                AmIExcludedFromPic = u.PrivacySettings.PrivacyExceptions
                    .Any(e => e.IsExcludedFromProfilePic &&e.ExcludedContactId == currentUserId)
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
        }
    }
}
