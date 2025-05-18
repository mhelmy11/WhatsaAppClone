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

    public virtual DbSet<Chat> Chats { get; set; }

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

        modelBuilder.Entity<UserContact>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ContactId }).HasName("PK_UserContact");

            entity.HasIndex(e => e.UserId, "IX_UserContacts_UserId");

            entity.HasOne(d => d.Contact).WithMany(p => p.UserContactContactUsers)
                .HasForeignKey(d => d.ContactId)
                .HasConstraintName("FK_UserContacts_AspNetUsers2");

            entity.HasOne(d => d.User).WithMany(p => p.UserContactUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserContacts_AspNetUsers");
        });


        modelBuilder.Entity<Chat>(entity =>
        {

            entity.Property(e => e.LastMessageTime).HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ReceiverChats)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Chats_AspNetUsers1");

            entity.HasOne(d => d.Sender).WithMany(p => p.SenderChats)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Chats_AspNetUsers");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Group");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.HasOne(d => d.Creator).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_Groups_AspNetUsers");
        });


        modelBuilder.Entity<UserConnection>(entity =>
        {
            entity.HasKey(uc => new { uc.UserId, uc.ConnectionId });


            entity.HasOne(d => d.User).WithMany(p => p.UserConnections)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserConnections_AspNetUsers");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {

            entity.HasKey(ug => new { ug.UserId, ug.GroupId });

            entity.HasOne(d => d.Group).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_UserGroups_Groups");

            entity.HasOne(d => d.User).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserGroups_AspNetUsers");
        });
        modelBuilder.Entity<Message>(entity =>
        {

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_Messages_Chats");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ReceiverMessages)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Messages_AspNetUsers");

            entity.HasOne(d => d.Sender).WithMany(p => p.SenderMessages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Messages_AspNetUsers2");
        });



        base.OnModelCreating(modelBuilder);

    }





}

