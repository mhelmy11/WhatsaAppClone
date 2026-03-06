using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class UserPrivacySetting
    {
        public long UserId { get; set; } 
        public User User { get; set; }

        // : Everyone, MyContacts, Nobody
        public string LastSeenPrivacy { get; set; } = PrivacyLevel.Everyone;
        public string ProfilePicPrivacy { get; set; } = PrivacyLevel.Everyone;
        public string StatusPrivacy { get; set; } = PrivacyLevel.MyContacts;
        public string AboutPrivacy { get; set; } = PrivacyLevel.Everyone;
        public string OnlinePrivacy { get; set; } = PrivacyLevel.Everyone;

        public bool ReadReceipts { get; set; } = true;

        // Navigation property for Excluded 
        public ICollection<PrivacyException> PrivacyExceptions { get; set; } = new HashSet<PrivacyException>();
    }

    public class PrivacyException
    {
        public long OwnerUserId { get; set; } 
        public long ExcludedContactId { get; set; } 
        public bool IsExcludedFromProfilePic { get; set; } = false;
        public bool IsExcludedFromAbout { get; set; } = false;
        public bool IsExcludedFromLastSeen { get; set; } = false;
        public bool IsExcludedFromStatus { get; set; } = false;
        public bool IsExcludedFromOnlineStatus { get; set; } = false;
        public bool IsIncludedInStatus { get; set; } = false;
        public UserPrivacySetting? OwnerSettings { get; set; }
    }

    public static class PrivacyLevel
    {
        public const string Everyone = "Everyone";
        public const string MyContacts = "MyContacts";
        public const string Nobody = "Nobody";
        public const string SameAsLastseen = "SameAsLastseen";
        public const string MyContactsExcept = "MyContactsExcept";
        public const string OnlyShareWith = "OnlyShareWith";

    }
}
