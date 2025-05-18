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
        public bool IsActive { get; set; } = false;
        public DateTime activeAt { get; set; } = DateTime.Now;

        public DateTime lastSeen { get; set; } = DateTime.Now;


        //Blacklist navigations

        public virtual ICollection<Blacklist> BlockedUsers { get; set; } = new List<Blacklist>(); //my blocked users

        public virtual ICollection<Blacklist> BlockedByUsers { get; set; } = new List<Blacklist>();//users that blocked me




        public virtual ICollection<Chat> ReceiverChats { get; set; } = new List<Chat>(); //all chats
        public virtual ICollection<Chat> SenderChats { get; set; } = new List<Chat>(); //all chats
        #region oldNav
        //public virtual ICollection<Chat> ChatReceivers { get; set; } = new List<Chat>();

        //public virtual ICollection<Chat> ChatSenders { get; set; } = new List<Chat>(); 
        #endregion


        public virtual ICollection<Message> Messages { get; set; } = new List<Message>(); //all messages
        public virtual ICollection<Message> SenderMessages { get; set; } = new List<Message>(); //all messages
        public virtual ICollection<Message> ReceiverMessages { get; set; } = new List<Message>(); //all messages



        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

        public virtual ICollection<UserConnection> UserConnections { get; set; } = new List<UserConnection>();

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();


        public virtual ICollection<UserContact> UserContactContactUsers { get; set; } = new List<UserContact>();

        public virtual ICollection<UserContact> UserContactUsers { get; set; } = new List<UserContact>();


        public string? PicUrl { get; set; }

    }
}
