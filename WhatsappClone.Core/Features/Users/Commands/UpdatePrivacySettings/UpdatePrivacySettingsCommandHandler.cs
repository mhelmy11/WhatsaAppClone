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
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service.Helpers;
using EFCore.BulkExtensions;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Core.Features.Users.Commands.UpdatePrivacySettings
{
    public class UpdatePrivacySettingsCommandHandler : ResponseHandler, IRequestHandler<UpdatePrivacySettingsCommand, Response<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly SqlDBContext dBContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUserService currentUserService;

        public UpdatePrivacySettingsCommandHandler(
            UserManager<User> userManager,
            SqlDBContext dBContext,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService
            )
        {
            this.userManager = userManager;
            this.dBContext = dBContext;
            this.httpContextAccessor = httpContextAccessor;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<string>> Handle(UpdatePrivacySettingsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.UserId;
            var currentUserPrivacySettings = await dBContext.UserPrivacySettings
                .Include(p => p.PrivacyExceptions)
                .FirstOrDefaultAsync(u => u.UserId == currentUserId , cancellationToken);


            if(currentUserPrivacySettings is null)
            {
                return BadRequest<string>("Privacy settings not found.");
            }

            if (request.AboutPrivacy is not null) 
            {
                if(request.AboutPrivacy.WhoCanSeeMyAbout is not null)
                { 
                    currentUserPrivacySettings.AboutPrivacy = request.AboutPrivacy.WhoCanSeeMyAbout;
                }
                if(request.AboutPrivacy.MyContactsExcept is not null && request.AboutPrivacy.WhoCanSeeMyAbout == PrivacyLevel.MyContactsExcept)
                {
                    await dBContext.PrivacyExceptions
                    .Where(e => e.OwnerUserId == currentUserId)
                    .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsExcludedFromAbout, false), cancellationToken);


                    var exceptionsToUpsert = request.AboutPrivacy.MyContactsExcept.Select(contactId => new PrivacyException
                    {
                        OwnerUserId = currentUserId,
                        ExcludedContactId = contactId,
                        IsExcludedFromAbout = true,
                    }).ToList();

                    var bulkConfig = new BulkConfig
                    {
                        UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                        PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsExcludedFromAbout) }
                    };

                    await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);


                }
            }
            if (request.ProfilePicturePrivacy is not null) 
            {
                if(request.ProfilePicturePrivacy.WhoCanSeeMyProfilePicture is not null)
                { 
                    currentUserPrivacySettings.ProfilePicPrivacy = request.ProfilePicturePrivacy.WhoCanSeeMyProfilePicture;
                }
                if(request.ProfilePicturePrivacy.MyContactsExcept is not null && request.ProfilePicturePrivacy.WhoCanSeeMyProfilePicture == PrivacyLevel.MyContactsExcept)
                {
                    await dBContext.PrivacyExceptions
                  .Where(e => e.OwnerUserId == currentUserId)
                  .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsExcludedFromProfilePic, false), cancellationToken);


                    var exceptionsToUpsert = request.ProfilePicturePrivacy.MyContactsExcept.Select(contactId => new PrivacyException
                    {
                        OwnerUserId = currentUserId,
                        ExcludedContactId = contactId,
                        IsExcludedFromProfilePic = true,
                    }).ToList();

                    var bulkConfig = new BulkConfig
                    {
                        UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                        PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsExcludedFromProfilePic) }
                    };

                    await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);


                }
            }
            if (request.StatusPrivacy is not null) 
            {
                if(request.StatusPrivacy.WhoCanSeeMyStatus is not null)
                { 
                    currentUserPrivacySettings.StatusPrivacy = request.StatusPrivacy.WhoCanSeeMyStatus;
                }
                if(request.StatusPrivacy.MyContactsExcept is not null && request.StatusPrivacy.WhoCanSeeMyStatus == PrivacyLevel.MyContactsExcept)
                {
                    await dBContext.PrivacyExceptions
                .Where(e => e.OwnerUserId == currentUserId)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsExcludedFromStatus, false), cancellationToken);


                    var exceptionsToUpsert = request.StatusPrivacy.MyContactsExcept.Select(contactId => new PrivacyException
                    {
                        OwnerUserId = currentUserId,
                        ExcludedContactId = contactId,
                        IsExcludedFromStatus = true,
                    }).ToList();

                    var bulkConfig = new BulkConfig
                    {
                        UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                        PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsExcludedFromStatus) }
                    };

                    await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);


                }
                if (request.StatusPrivacy.OnlyShareWith is not null && request.StatusPrivacy.WhoCanSeeMyStatus == PrivacyLevel.OnlyShareWith)
                {
                    await dBContext.PrivacyExceptions
             .Where(e => e.OwnerUserId == currentUserId)
             .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsIncludedInStatus, false), cancellationToken);


                    var exceptionsToUpsert = request.StatusPrivacy.OnlyShareWith.Select(contactId => new PrivacyException
                    {
                        OwnerUserId = currentUserId,
                        ExcludedContactId = contactId,
                        IsIncludedInStatus = true,
                    }).ToList();

                    var bulkConfig = new BulkConfig
                    {
                        UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                        PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsIncludedInStatus) }
                    };

                    await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);

                }


            }
            if (request.LastseenAndOnline is not null) 
            {
                if(request.LastseenAndOnline.WhoCanSeeMyLastseen is not null)
                { 
                    currentUserPrivacySettings.LastSeenPrivacy = request.LastseenAndOnline.WhoCanSeeMyLastseen;
                    if (request.LastseenAndOnline.MyContactsExcept is not null && request.LastseenAndOnline.WhoCanSeeMyLastseen == PrivacyLevel.MyContactsExcept)
                    {
                        await dBContext.PrivacyExceptions
                    .Where(e => e.OwnerUserId == currentUserId)
                    .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsExcludedFromLastSeen, false), cancellationToken);


                        var exceptionsToUpsert = request.LastseenAndOnline.MyContactsExcept.Select(contactId => new PrivacyException
                        {
                            OwnerUserId = currentUserId,
                            ExcludedContactId = contactId,
                            IsExcludedFromLastSeen = true,
                        }).ToList();

                        var bulkConfig = new BulkConfig
                        {
                            UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                            PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsExcludedFromLastSeen)}
                        };

                        await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);


                    }
                }
                if(request.LastseenAndOnline.WhoCanSeeWhenIAmOnline is not null)
                {
                    currentUserPrivacySettings.OnlinePrivacy = request.LastseenAndOnline.WhoCanSeeWhenIAmOnline;

                    if (request.LastseenAndOnline.WhoCanSeeWhenIAmOnline == PrivacyLevel.SameAsLastseen && request.LastseenAndOnline.MyContactsExcept is not null)
                    {
                        await dBContext.PrivacyExceptions
                        .Where(e => e.OwnerUserId == currentUserId)
                        .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsExcludedFromOnlineStatus, false), cancellationToken);


                        var exceptionsToUpsert = request.LastseenAndOnline.MyContactsExcept.Select(contactId => new PrivacyException
                        {
                            OwnerUserId = currentUserId,
                            ExcludedContactId = contactId,
                            IsExcludedFromOnlineStatus = true,
                        }).ToList();

                        var bulkConfig = new BulkConfig
                        {
                            UpdateByProperties = new List<string> { nameof(PrivacyException.OwnerUserId), nameof(PrivacyException.ExcludedContactId) },
                            PropertiesToIncludeOnUpdate = new List<string> { nameof(PrivacyException.IsExcludedFromOnlineStatus) }
                        };

                        await dBContext.BulkInsertOrUpdateAsync(exceptionsToUpsert, bulkConfig, cancellationToken: cancellationToken);
                    }
                    currentUserPrivacySettings.OnlinePrivacy = request.LastseenAndOnline.WhoCanSeeWhenIAmOnline;
                }
             
            }

            if(request.ReadReceipts.HasValue)
            {
                currentUserPrivacySettings.ReadReceipts = request.ReadReceipts.Value;
            }

            await dBContext.SaveChangesAsync(cancellationToken);

            return Success<string>(null, "Privacy Settings Updated Successfully");

        }
    }
}
