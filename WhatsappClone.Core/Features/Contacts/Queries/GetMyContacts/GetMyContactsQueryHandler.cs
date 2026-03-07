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
using WhatsappClone.Service.Abstract;
using WhatsappClone.Service.Helpers;
using WhatsappClone.Service.Helpers.WhatsappClone.Shared.Extensions;

namespace WhatsappClone.Core.Features.Contacts.Queries.GetMyContacts
{
    internal class GetMyContactsQueryHandler : ResponseHandler, IRequestHandler<GetMyContactsQuery, Response<CursorPagedResult<GetMyContactsResult>>>
    {
        private readonly SqlDBContext dBContext;
        private readonly ICurrentUserService currentUserService;

        public record ContactCursor(string Name, long Id);
        public GetMyContactsQueryHandler(
            SqlDBContext dBContext,
            ICurrentUserService currentUserService
        )
        {
            this.dBContext = dBContext;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<CursorPagedResult<GetMyContactsResult>>> Handle(GetMyContactsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var cursor = CursorHelper.Decode<ContactCursor>(request.Cursor);
            var query = dBContext.Contacts.AsNoTracking().Where(c => c.UserId == currentUserId);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(c => c.ContactName.Contains(request.SearchTerm));
            }

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

                        AmIInHisContacts = c.ContactUser.Contacts.Any(hisContact => hisContact.ContactUserId == currentUserId),

                        AmIExcludedFromPic = c.ContactUser.PrivacySettings.PrivacyExceptions.Any(e =>e.ExcludedContactId == currentUserId && e.IsExcludedFromProfilePic),

                        AmIExcludedFromAbout = c.ContactUser.PrivacySettings.PrivacyExceptions.Any(e => e.ExcludedContactId == currentUserId && e.IsExcludedFromAbout)
                    })
                    .ToListAsync(cancellationToken);

            var finalContacts = rawContacts.Select(raw => new GetMyContactsResult(
                    ContactUserId: raw.ContactId,
                    ContactName: raw.ContactName,
                    ProfilePicUrl: raw.RawProfilePicUrl.ResolveProfilePic(raw.PicPrivacyLevel, raw.AmIInHisContacts, raw.AmIExcludedFromPic),
                    About: raw.RawAbout.CanViewLastSeen(raw.AboutPrivacyLevel, raw.AmIInHisContacts, raw.AmIExcludedFromAbout) ? raw.RawAbout : null
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
