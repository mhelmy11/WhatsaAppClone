using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Helpers.WhatsappClone.Shared.Extensions;

namespace WhatsappClone.Core.Features.Users.Queries.GetBlockedUsers
{
    public class GetBlockedUsersQueryHandler : ResponseHandler, IRequestHandler<GetBlockedUsersQuery, Response<List<GetBlockedUsersResult>>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;

        public GetBlockedUsersQueryHandler(
            SqlDBContext dBContext,
            ICurrentUserService currentUserService)
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<List<GetBlockedUsersResult>>> Handle(GetBlockedUsersQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var query = dBContext.BlockedUsers.AsNoTracking().Where(c => c.UserId == currentUserId);
            var rawBlockedUsers = await query
                    .OrderBy(c => c.BlockedAt)
                    .ThenBy(c => c.BlockedUserId)
                    .Select(c => new
                    {
                        BlockedUserId = c.BlockedUserId,
                        ContactName = c.User.Contacts
                            .Where(u => u.ContactUserId == c.BlockedUserId)
                            .Select(u => u.ContactName)
                            .FirstOrDefault() ,
                        PhoneNumber = c.Blocked.PhoneNumber,
                        CountryCode = c.Blocked.CountryCode,    
                        RawProfilePicUrl = c.Blocked.ProfilePicUrl,
                        PicPrivacyLevel = c.Blocked.PrivacySettings.ProfilePicPrivacy,
                        AmIInHisContacts = c.Blocked.Contacts.Any(hisContact => hisContact.ContactUserId == currentUserId),
                        AmIExcludedFromPic = c.Blocked.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == currentUserId && e.IsExcludedFromProfilePic),

                    })
                    .ToListAsync(cancellationToken);

            var finalBlockedUsers =  rawBlockedUsers.Select(b=> new GetBlockedUsersResult
            {
                CountryCode = b.CountryCode,
                Name = b.ContactName ?? $"{b.CountryCode} {b.PhoneNumber}",
                PhoneNumber = b.PhoneNumber,
                ProfilePic = b.RawProfilePicUrl.ResolveProfilePic(b.PicPrivacyLevel,b.AmIInHisContacts,b.AmIExcludedFromPic),
                BlockedUserId = b.BlockedUserId
            })
            .ToList();

            return Success(finalBlockedUsers);

        }
    }
}
