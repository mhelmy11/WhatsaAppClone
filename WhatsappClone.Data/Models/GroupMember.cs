using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappClone.Data.Models;

public class GroupMember
{
    public long GroupId { get; set; }
    public long UserId { get; set; }


    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }


    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public string Role { get; set; } = MemberRole.Member; 

    public string? TagName { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

  

    public string Status { get; set; }  = MemberStatus.Active; // 1=active, 0=left, 2=removed

    public DateTime? LeftAt { get; set; }

}

public static class MemberRole
{
    public const string Member = "Member";
    public const string Admin = "Admin";
}
public static class MemberStatus
{
    public const string Active = "Active";
    public const string Left = "Left";
    public const string Removed = "Removed";
}
