using Microsoft.EntityFrameworkCore;
using Notebook.Entities.Entities;
using System;

namespace Notebook.DataAccess.DataContext
{
    public class NotebookContext : DbContext
    {
        public NotebookContext(DbContextOptions<NotebookContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<UserNote> UserNote { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Follow> Follow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Hash Data

            Role role = new Role
            {
                ID = "56854644",
                Name = "Admin",
                Permissions = "VIEW_ADMINPANEL,VIEW_ROLE,ADD_ROLE,EDIT_ROLE,DELETE_ROLE,EDIT_SETTINGS,VIEW_USERS,EDIT_USERS,DELETE_USERS"
            };

            modelBuilder.Entity<Role>().HasData(role);

            User user = new User
            {
                Name = "Muammer Hafızoğlu",
                Username = "muammer.hafizoglu",
                Approve = true,
                CreateDate = DateTime.Now,
                Email = "mhaf69@gmail.com",
                Password = "D3CE20FCCBE7D116ECD0",
                ID = "23m454h5",
                Avatar = "/notebook/images/avatar.png",
                LastActiveDate = DateTime.Now
            };

            modelBuilder.Entity<User>().HasData(user);

            Settings settings = new Settings
            {
                ID = "45634df5",
                Title = "Notebook",
                WebAddress = "https://www.notebook.com",
                DefaultLanguage = "en-EN",
                Logo = "/notebook/images/logo.png",
                Icon = "/favicon.ico",
                IsMembershipOpen = true,
                MembershipEmailControl = false,
                Footer = "Copyright © 2014-2016 Almsaeed Studio. All rights reserved. "
            };

            modelBuilder.Entity<Settings>().HasData(settings);

            #endregion

            #region Relationship

            modelBuilder.Entity<Follow>()
                        .HasKey(a => new { a.FollowerID, a.FollowingID });
            modelBuilder.Entity<Follow>()
                        .HasOne(a => a.Follower)
                        .WithMany(a => a.Following)
                        .HasForeignKey(a => a.FollowerID);
            modelBuilder.Entity<Follow>()
                        .HasOne(a => a.Following)
                        .WithMany(a => a.Follower)
                        .HasForeignKey(a => a.FollowingID);

            modelBuilder.Entity<UserGroup>()
                        .HasKey(a => new { a.UserID, a.GroupID });
            modelBuilder.Entity<UserGroup>()
                        .HasOne(a => a.User)
                        .WithMany(a => a.Groups)
                        .HasForeignKey(a => a.UserID)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserGroup>()
                        .HasOne(a => a.Group)
                        .WithMany(a => a.Users)
                        .HasForeignKey(a => a.GroupID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserNote>()
                        .HasKey(a => new { a.UserID, a.NoteID });
            modelBuilder.Entity<UserNote>()
                        .HasOne(a => a.User)
                        .WithMany(a => a.Notes)
                        .HasForeignKey(a => a.UserID);
            modelBuilder.Entity<UserNote>()
                        .HasOne(a => a.Note)
                        .WithMany(a => a.Users)
                        .HasForeignKey(a => a.NoteID);

            modelBuilder.Entity<User>()
                       .HasMany(c => c.Groups)
                       .WithOne(e => e.User)
                       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                        .HasMany(c => c.Notes)
                        .WithOne(e => e.User)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                        .HasMany(c => c.Notes)
                        .WithOne(e => e.Group)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Folder>()
                        .HasMany(c => c.Notes)
                        .WithOne(e => e.Folder)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                        .HasMany(c => c.Folders)
                        .WithOne(e => e.Group)
                        .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
