using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.AspNetCore.Identity;
namespace WhatsappClone.Data.Models
{
    public class User : IdentityUser<long>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; } 
        public string CountryCode { get; set; }
        public string ProfilePicUrl { get; set; } = "https://example.com/default-profile-pic.jpg";
        public string About { get; set; }= "Hey there! I am using WhatsApp.";
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeen { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public bool AllowReadReceipts { get; set; } = true;
        public string WhoCanSeeMyStory { get; set; } = "Contacts";
        public string WhoCanAddMeToGroups { get; set; } = "Everyone";
        public string WhoCanSeeMyLastSeen { get; set; } = "Everyone";
        public string WhoCanSeeMyOnlineStatus { get; set; } = "Everyone";
        public string WhoCanViewProfilePic { get; set; } = "Everyone";
        public string PreferredLanguage { get; set; } = "English";
        public string ThemePreference { get; set; } = "Light";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        //Relations
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<UserConnection> UserConnections { get; set; }
        public ICollection<RefreshTokenAudit> RefreshTokenAudits { get; set; }
        public ICollection<Contact> ContactOf { get; set; }
        public ICollection<BlockedUser> BlockedUsers { get; set; }
        public ICollection<BlockedUser> BlockedBy { get; set; }
        public ICollection<Group> CreatedGroups { get; set; }
        public ICollection<GroupMember> GroupMemberships { get; set; }
    }


    public class Role : IdentityRole<long>
    {

    }



}
