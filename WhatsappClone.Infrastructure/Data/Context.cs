using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Infrastructure;

public class Context : IdentityDbContext<AppUser>
{
    public virtual DbSet<Blacklist> Blacklists { get; set; }
    public virtual DbSet<TokenRefreshing> RefreshTokens { get; set; }
    // Status moved to MongoDB - use IMongoCollection<Status> instead
    public virtual DbSet<Attachments> Attachments { get; set; }
    public virtual DbSet<UserChatSettings> UserChatSettings { get; set; }
    public virtual DbSet<MessageReadStatus> MessageReadStatuses { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<UserConnection> UserConnections { get; set; }
    public virtual DbSet<UserGroup> UserGroups { get; set; }
    public virtual DbSet<UserContact> UserContacts { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Attachments>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Message).WithMany(e => e.Attachments).HasForeignKey(e => e.MessageId);

        });
        modelBuilder.Entity<Blacklist>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BlockedUserId }).HasName("PK_Blacklist");

            entity.HasIndex(e => e.UserId, "IX_Blacklists_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.BlockedUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Blacklists_AspNetUsers2");

            entity.HasOne(d => d.BlockedUser).WithMany(p => p.BlockedByUsers)
                .HasForeignKey(d => d.BlockedUserId)
                .HasConstraintName("FK_Blacklists_AspNetUsers");
        });



        modelBuilder.Entity<UserContact>().HasKey(c => new { c.UserId, c.ContactId });
        modelBuilder.Entity<UserConnection>().HasKey(c => new { c.UserId, c.ConnectionId });
        modelBuilder.Entity<UserGroup>().HasKey(gm => new { gm.GroupId, gm.UserId });
        modelBuilder.Entity<MessageReadStatus>().HasKey(mrs => new { mrs.MessageId, mrs.UserId });


        modelBuilder.Entity<UserContact>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserContact>()
            .HasOne(c => c.Contact)
            .WithMany(u => u.ContactsOf)
            .HasForeignKey(c => c.ContactId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<UserChatSettings>(entity =>
        {
            entity.HasOne(s => s.User).WithMany(u => u.ChatSettings).HasForeignKey(s => s.UserId);
            entity.HasOne(s => s.Contact).WithMany().HasForeignKey(s => s.ContactId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.Group).WithMany(u => u.ChatSettings).HasForeignKey(s => s.GroupId);


        });



        base.OnModelCreating(modelBuilder);

    }


    //{
    //"type":"MEMBER_ADDED",
    //"actorUserId":"474a5eb7-c051-4ef4-aa6d-01245d374f82"
    //,"targetUserIds":["474a5eb7-c051-4ef4-aa6d-01245d374f82"
    //,"f4a60247-6a19-4f2d-a050-65e91013b704"]}



}

