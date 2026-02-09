using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.SqlServerModels;

namespace WhatsappClone.Infrastructure;

public class SqlDBContext : IdentityDbContext<AppUser>
{
    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<Blacklist> Blacklists { get; set; }
    public virtual DbSet<RefreshTokenAudit> RefreshTokenAudits { get; set; }
    public virtual DbSet<UserChatSettings> UserChatSettings { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<UserConnection> UserConnections { get; set; }
    public virtual DbSet<UserGroup> UserGroups { get; set; }
    public virtual DbSet<UserContact> UserContacts { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }


    public SqlDBContext(DbContextOptions<SqlDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============ AppUser Configuration ============
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.PhoneNumber);
            entity.HasIndex(e => e.LastSeen);
        });

        // ============ Blacklist Configuration ============
        modelBuilder.Entity<Blacklist>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BlockedUserId })
                .HasName("PK_Blacklist");
            entity.HasIndex(e => e.BlockedUserId, "IX_Blacklists_BlockedUserId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.BlockedUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Blacklists_AspNetUsers2");

            entity.HasOne(d => d.BlockedUser)
                .WithMany(p => p.BlockedByUsers)
                .HasForeignKey(d => d.BlockedUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Blacklists_AspNetUsers");
        });

        // ============ Chat Configuration ============
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasIndex(e => e.SenderId);
            entity.HasIndex(e => e.ReceiverId);
            entity.HasIndex(e => e.GroupId).HasFilter("[ChatType] = 'Group'");
            entity.HasIndex(e => e.LastMessageTime).HasFilter("[IsDeleted] = 0");
            entity.HasIndex(e => e.IsDeleted);

            entity.HasOne(e => e.Sender)
                .WithMany()
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============ Group Configuration ============
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasIndex(e => e.CreatorId);
            entity.HasIndex(e => e.InviteCode).IsUnique();

            entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============ RefreshTokenAudit Configuration ============
        modelBuilder.Entity<RefreshTokenAudit>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => e.IsRevoked);

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============ UserChatSettings Configuration ============
        modelBuilder.Entity<UserChatSettings>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => new { e.UserId, e.ChatId });
            entity.HasIndex(e => e.IsPinned);
            entity.HasIndex(e => e.IsArchived);
            entity.HasIndex(e => e.IsFavorite);
            entity.HasIndex(e => e.IsDeleted);

            entity.HasOne(s => s.User)
                .WithMany(u => u.ChatSettings)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Chat)
                .WithMany()
                .HasForeignKey(s => s.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============ UserContact Configuration ============
        modelBuilder.Entity<UserContact>(entity =>
        {
            entity.HasKey(c => new { c.UserId, c.ContactId });

            entity.HasIndex(c => c.ContactId);
            entity.HasIndex(c => c.UserId);
            entity.HasIndex(c => new { c.UserId, c.ContactId });

            entity.HasOne(c => c.User)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Contact)
                .WithMany(u => u.ContactsOf)
                .HasForeignKey(c => c.ContactId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ============ UserConnection Configuration ============
        modelBuilder.Entity<UserConnection>(entity =>
        {
            entity.HasKey(uc => new { uc.UserId, uc.ConnectionId });

            entity.HasOne(uc => uc.User)
                .WithMany(u => u.UserConnections)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ============ UserGroup Configuration ============
        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(ug => new { ug.GroupId, ug.UserId });

            entity.HasIndex(ug => ug.UserId);
            entity.HasIndex(ug => ug.IsMember);
            entity.HasIndex(ug => new { ug.GroupId, ug.IsMember });
            entity.HasIndex(ug => ug.Role);

            entity.HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }


    //{
    //"type":"MEMBER_ADDED",
    //"actorUserId":"474a5eb7-c051-4ef4-aa6d-01245d374f82"
    //,"targetUserIds":["474a5eb7-c051-4ef4-aa6d-01245d374f82"
    //,"f4a60247-6a19-4f2d-a050-65e91013b704"]}



}

