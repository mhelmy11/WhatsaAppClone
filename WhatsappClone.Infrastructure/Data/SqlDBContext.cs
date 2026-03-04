using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Models;

namespace WhatsappClone.Infrastructure;

public class SqlDBContext : IdentityDbContext<User,Role,long>
{
    public virtual DbSet<BlockedUser> BlockedUsers { get; set; }
    public virtual DbSet<RefreshTokenAudit> RefreshTokenAudits { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<UserConnection> UserConnections { get; set; }
    public virtual DbSet<GroupMember> GroupMembers { get; set; }
    public virtual DbSet<GroupJoinRequest> GroupJoinRequests { get; set; }
    public virtual DbSet<Contact> Contacts { get; set; }
    public virtual DbSet<UserPrivacySetting> UserPrivacySettings { get; set; }


    public SqlDBContext(DbContextOptions<SqlDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============ User Configuration ============
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.PhoneNumber);
            entity.HasIndex(e => e.Email);
        });

        modelBuilder.Entity<UserPrivacySetting>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasOne(x => x.User)
                   .WithOne(u => u.PrivacySettings)
                   .HasForeignKey<UserPrivacySetting>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

        });
        modelBuilder.Entity<PrivacyException>(entity =>
        {
            entity.HasKey(k => new { k.OwnerUserId , k.ExcludedContactId });
            entity.HasOne(x=>x.OwnerSettings)
                   .WithMany(p => p.PrivacyExceptions)
                   .HasForeignKey(x => x.OwnerUserId)
                   .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.ExcludedContactId);

        });

        //============ Blacklist Configuration ============
        modelBuilder.Entity<BlockedUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BlockedUserId });
            entity.HasIndex(e => e.BlockedUserId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.BlockedUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d=>d.Blocked)
            .WithMany(p=>p.BlockedBy)
            .HasForeignKey(e=>e.BlockedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        });


        // ============ Group Configuration ============
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasIndex(e => e.InviteLink).IsUnique();

            entity.HasOne(e => e.Creator)
                .WithMany(e=>e.CreatedGroups)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);


            entity.HasMany(e => e.JoinRequests)
                .WithOne(e=>e.Group)
                .HasForeignKey(e => e.GroupId)
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
                .WithMany(u => u.RefreshTokenAudits)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

     

        // ============ Contact Configuration ============
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(c => new { c.UserId, c.ContactUserId });

            entity.HasIndex(c => c.ContactUserId);
            entity.HasIndex(c => c.UserId);
            entity.HasIndex(c => new { c.UserId, c.ContactUserId });

            entity.HasOne(c => c.User)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ContactUser)
                .WithMany(u => u.ContactOf)
                .HasForeignKey(c => c.ContactUserId)
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

        // ============ GroupMember Configuration ============
        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(ug => new { ug.GroupId, ug.UserId });

            entity.HasIndex(ug => ug.UserId);
            entity.HasIndex(ug => ug.Role);

            entity.HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ug => ug.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }


    

}

