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
using WhatsappClone.Core.Wrappers;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Helpers.WhatsappClone.Shared.Extensions;

namespace WhatsappClone.Core.Features.Contacts.Queries.GetMyContacts
{
    internal class GetMyContactsQueryHandler : ResponseHandler, IRequestHandler<GetMyContactsQuery, Response<CursorPagedResult<GetMyContactsResult>>>
    {
        private readonly SqlDBContext dBContext;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public record ContactCursor(string Name, long Id);
        public GetMyContactsQueryHandler(SqlDBContext dBContext , UserManager<User> userManager , IHttpContextAccessor httpContextAccessor)
        {
            this.dBContext = dBContext;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<CursorPagedResult<GetMyContactsResult>>> Handle(GetMyContactsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await userManager.GetCurrentUser(httpContextAccessor);
            var cursor = CursorHelper.Decode<ContactCursor>(request.Cursor);
            var query = dBContext.Contacts.AsNoTracking().Where(c => c.UserId == currentUser.Id);

            if (cursor != null)
            {
                query = query.Where(c =>
                    c.ContactName.CompareTo(cursor.Name) > 0 ||
                    (c.ContactName == cursor.Name && c.ContactUserId > cursor.Id));
            }
            var rawContacts = await query
                    .OrderBy(c => c.ContactName)
                    .ThenBy(c => c.ContactUserId)
                    .Take(request.Limit + 1)
                    .Select(c => new
                    {
                        ContactId = c.ContactUserId,
                        ContactName = c.ContactName,
                        PhoneNumber = c.ContactUser.PhoneNumber,
                        RawProfilePicUrl = c.ContactUser.ProfilePicUrl,
                        RawAbout = c.ContactUser.About,

                        PicPrivacyLevel = c.ContactUser.PrivacySettings.ProfilePicPrivacy,
                        AboutPrivacyLevel = c.ContactUser.PrivacySettings.AboutPrivacy,

                        AmIInHisContacts = dBContext.Contacts.Any(hisContact =>
                            hisContact.UserId == c.ContactUserId &&
                            hisContact.ContactUserId == currentUser.Id),

                        AmIExcludedFromPic = dBContext.PrivacyExceptions.Any(e =>
                            e.OwnerUserId == c.ContactUserId &&
                            e.ExcludedContactId == currentUser.Id &&
                            e.IsExcludedFromProfilePic),

                        AmIExcludedFromAbout = dBContext.PrivacyExceptions.Any(e =>
                            e.OwnerUserId == c.ContactUserId &&
                            e.ExcludedContactId == currentUser.Id &&
                            e.IsExcludedFromAbout)
                    })
                    .ToListAsync(cancellationToken);

            var finalContacts = rawContacts.Select(raw => new GetMyContactsResult(
                    ContactUserId: raw.ContactId,
                    ContactName: raw.ContactName,
                    ProfilePicUrl: raw.RawProfilePicUrl.ResolveProfilePic(raw.PicPrivacyLevel, raw.AmIInHisContacts, raw.AmIExcludedFromPic),
                    About: raw.RawAbout.CanViewLastSeen(raw.AboutPrivacyLevel, raw.AmIInHisContacts, raw.AmIExcludedFromAbout)
                   ))
                    .ToList();

            var pagedResult = new CursorPagedResult<GetMyContactsResult>(
                finalContacts,
                request.Limit,
                lastItem => CursorHelper.Encode(new ContactCursor(lastItem.ContactName, lastItem.ContactUserId))
        );

            return Success(pagedResult);
        }
    }
}
