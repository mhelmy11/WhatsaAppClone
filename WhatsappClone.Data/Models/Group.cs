using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; } //snowflakeId

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ProfilePicUrl { get; set; } = "default_group_avatar";

        public long CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User Creator { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //settings
        public bool AllowMessagesForMembers { get; set; } = false;

        public bool ApprovalRequiredToJoin { get; set; } = false;
        public bool EditGroupDescription { get; set; } = true;


        public bool IsCommunity { get; set; } = false;

        public long? CommunityId { get; set; }
        [MaxLength(255)]
        public string InviteLink { get; set; } = "";

        public DateTime? InviteLinkExpiry { get; set; }

        public bool IsArchived { get; set; } = false;

        public DateTime? ArchivedAt { get; set; }

        // Navigation
        public ICollection<GroupMember> Members { get; set; } = new HashSet<GroupMember>();
        public ICollection<GroupJoinRequest> JoinRequests { get; set; } = new HashSet<GroupJoinRequest>();
    }

}
