using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;

namespace WhatsappClone.Core.Features.Users.Commands.UpdatePrivacySettings
{
    public record UpdatePrivacySettingsCommand(
        LastseenAndOnlinePrivacy? LastseenAndOnline ,
        ProfilePicturePrivacy? ProfilePicturePrivacy ,
        AboutPrivacy? AboutPrivacy,
        StatusPrivacy? StatusPrivacy ,
        bool? ReadReceipts
        ) : IRequest<Response<string>>
    {
        
    }

    public class LastseenAndOnlinePrivacy
    {
        public string? WhoCanSeeMyLastseen { get; set; }
        public List<long>? MyContactsExcept { get; set; }
        public string? WhoCanSeeWhenIAmOnline { get; set; }

    }

    public class ProfilePicturePrivacy
    {
        public string? WhoCanSeeMyProfilePicture { get; set; }

        public List<long>? MyContactsExcept {  get; set; }
    }
    public class AboutPrivacy
    {
        public string? WhoCanSeeMyAbout { get; set; }

        public List<long>? MyContactsExcept {  get; set; }
    }
    public class StatusPrivacy
    {
        public string? WhoCanSeeMyStatus { get; set; }
        public List<long>? MyContactsExcept {  get; set; }
        public List<long>? OnlyShareWith {  get; set; }
    }



}
