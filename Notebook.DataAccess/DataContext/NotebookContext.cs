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
                        .WithMany(a => a.Followers)
                        .HasForeignKey(a => a.FollowerID);
            modelBuilder.Entity<Follow>()
                        .HasOne(a => a.Following)
                        .WithMany(a => a.Following)
                        .HasForeignKey(a => a.FollowingID);

            modelBuilder.Entity<UserGroup>()
                        .HasKey(a => new { a.UserID, a.GroupID });
            modelBuilder.Entity<UserGroup>()
                        .HasOne(a => a.User)
                        .WithMany(a => a.SucscribedGroups)
                        .HasForeignKey(a => a.UserID);
            modelBuilder.Entity<UserGroup>()
                        .HasOne(a => a.Group)
                        .WithMany(a => a.Users)
                        .HasForeignKey(a => a.GroupID);

            //modelBuilder.Entity<UserFolder>()
            //            .HasKey(a => new { a.UserID, a.FolderID });
            //modelBuilder.Entity<UserFolder>()
            //            .HasOne(a => a.User)
            //            .WithMany(a => a.SucscribedFolders)
            //            .HasForeignKey(a => a.UserID);
            //modelBuilder.Entity<UserFolder>()
            //            .HasOne(a => a.Folder)
            //            .WithMany(a => a.Users)
            //            .HasForeignKey(a => a.FolderID);

            //modelBuilder.Entity<UserNote>()
            //            .HasKey(a => new { a.UserID, a.NoteID });
            //modelBuilder.Entity<UserNote>()
            //            .HasOne(a => a.User)
            //            .WithMany(a => a.SucscribedNotes)
            //            .HasForeignKey(a => a.UserID);
            //modelBuilder.Entity<UserNote>()
            //            .HasOne(a => a.Note)
            //            .WithMany(a => a.Users)
            //            .HasForeignKey(a => a.NoteID);

            //modelBuilder.Entity<GroupFolder>()
            //            .HasKey(a => new { a.GroupID, a.FolderID });
            //modelBuilder.Entity<GroupFolder>()
            //            .HasOne(a => a.Group)
            //            .WithMany(a => a.Folders)
            //            .HasForeignKey(a => a.GroupID);
            //modelBuilder.Entity<GroupFolder>()
            //            .HasOne(a => a.Folder)
            //            .WithMany(a => a.Groups)
            //            .HasForeignKey(a => a.FolderID);

            //modelBuilder.Entity<GroupNote>()
            //            .HasKey(a => new { a.GroupID, a.NoteID });
            //modelBuilder.Entity<GroupNote>()
            //            .HasOne(a => a.Group)
            //            .WithMany(a => a.Notes)
            //            .HasForeignKey(a => a.GroupID);
            //modelBuilder.Entity<GroupNote>()
            //            .HasOne(a => a.Note)
            //            .WithMany(a => a.Groups)
            //            .HasForeignKey(a => a.NoteID);

            //modelBuilder.Entity<FolderNote>()
            //            .HasKey(a => new { a.FolderID, a.NoteID });
            //modelBuilder.Entity<FolderNote>()
            //            .HasOne(a => a.Folder)
            //            .WithMany(a => a.Notes)
            //            .HasForeignKey(a => a.FolderID);
            //modelBuilder.Entity<FolderNote>()
            //            .HasOne(a => a.Note)
            //            .WithMany(a => a.Folders)
            //            .HasForeignKey(a => a.NoteID);

            #endregion
        }
    }
}
