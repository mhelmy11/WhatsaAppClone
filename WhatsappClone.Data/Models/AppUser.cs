using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatsappClone.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime activeAt { get; set; } = DateTime.UtcNow;

        public DateTime lastSeen { get; set; } = DateTime.UtcNow;

        public string? About { get; set; } = "Hey there! I'm using WhatsappClone!";
        public string? PicUrl { get; set; }


        //Blacklist navigations

        public virtual ICollection<Blacklist> BlockedUsers { get; set; } = new HashSet<Blacklist>(); //my blocked users

        public virtual ICollection<Blacklist> BlockedByUsers { get; set; } = new HashSet<Blacklist>();//users that blocked me
        public virtual ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>(); //all messages
        public virtual ICollection<UserContact> Contacts { get; set; } = new HashSet<UserContact>();
        public virtual ICollection<UserContact> ContactsOf { get; set; } = new HashSet<UserContact>();
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
        public virtual ICollection<Status> Statuses { get; set; } = new HashSet<Status>();
        public virtual ICollection<UserChatSettings> ChatSettings { get; set; } = new HashSet<UserChatSettings>();
        public virtual ICollection<UserConnection> UserConnections { get; set; } = new HashSet<UserConnection>();
        public virtual ICollection<TokenRefreshing> UserRefreshTokens { get; set; } = new HashSet<TokenRefreshing>();







    }
}
