﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notebook.DataAccess.DataContext;

namespace Notebook.DataAccess.Migrations
{
    [DbContext(typeof(NotebookContext))]
    [Migration("20190929025621_a")]
    partial class a
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Notebook.Entities.Entities.Calendar", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<string>("Content");

                    b.Property<DateTime>("FinishDate");

                    b.Property<string>("Location");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Title");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Calendar");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Event", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Explation");

                    b.Property<int>("Operation");

                    b.Property<string>("ProductID");

                    b.Property<string>("ProductName");

                    b.Property<int>("Type");

                    b.Property<string>("Url");

                    b.Property<string>("UserID");

                    b.Property<bool>("View");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Folder", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Explanation");

                    b.Property<string>("GroupID");

                    b.Property<string>("Name");

                    b.Property<int>("Visible");

                    b.HasKey("ID");

                    b.HasIndex("GroupID");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Follow", b =>
                {
                    b.Property<string>("FollowerID");

                    b.Property<string>("FollowingID");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("ID")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("Notification");

                    b.Property<int>("Status");

                    b.HasKey("FollowerID", "FollowingID");

                    b.HasAlternateKey("ID");

                    b.HasIndex("FollowingID");

                    b.ToTable("Follow");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Group", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Explanation");

                    b.Property<bool>("IsRequiredApproval");

                    b.Property<string>("Name");

                    b.Property<string>("UserID");

                    b.Property<int>("Visible");

                    b.HasKey("ID");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Log", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Detail");

                    b.HasKey("ID");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Note", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<string>("Content");

                    b.Property<int>("CopyCount");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Explanation");

                    b.Property<string>("FolderID");

                    b.Property<string>("GroupID");

                    b.Property<bool>("OpenToComments");

                    b.Property<bool>("OpenToCopy");

                    b.Property<int>("ReadCount");

                    b.Property<string>("Tags");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<string>("UserID");

                    b.Property<int>("Visible");

                    b.HasKey("ID");

                    b.HasIndex("FolderID");

                    b.HasIndex("GroupID");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Permission", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<bool>("IsModule");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Role", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<string>("Name");

                    b.Property<string>("Permissions");

                    b.HasKey("ID");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            ID = "56854644",
                            Name = "Admin",
                            Permissions = "VIEW_ADMINPANEL,VIEW_ROLE,ADD_ROLE,EDIT_ROLE,DELETE_ROLE,EDIT_SETTINGS,VIEW_USERS,EDIT_USERS,DELETE_USERS"
                        });
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Settings", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<string>("Address");

                    b.Property<string>("DefaultLanguage");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Facebook");

                    b.Property<string>("Footer");

                    b.Property<string>("Gmail");

                    b.Property<string>("Host");

                    b.Property<string>("Icon");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsMailActive");

                    b.Property<bool>("IsMembershipOpen");

                    b.Property<string>("Linkedin");

                    b.Property<string>("Logo");

                    b.Property<bool>("MembershipEmailControl");

                    b.Property<string>("Metadata");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<string>("Port");

                    b.Property<string>("SingleFileSize");

                    b.Property<string>("Title");

                    b.Property<string>("TotalFileSize");

                    b.Property<string>("Twitter");

                    b.Property<string>("Username");

                    b.Property<string>("WebAddress");

                    b.HasKey("ID");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            ID = "45634df5",
                            DefaultLanguage = "en-EN",
                            Footer = "Copyright © 2014-2016 Almsaeed Studio. All rights reserved. ",
                            Icon = "/favicon.ico",
                            IsMailActive = false,
                            IsMembershipOpen = true,
                            Logo = "/notebook/images/logo.png",
                            MembershipEmailControl = false,
                            Title = "Notebook",
                            WebAddress = "https://www.notebook.com"
                        });
                });

            modelBuilder.Entity("Notebook.Entities.Entities.User", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<bool>("Approve");

                    b.Property<string>("Avatar");

                    b.Property<bool>("CanUploadFile");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Email");

                    b.Property<string>("Info");

                    b.Property<DateTime>("LastActiveDate");

                    b.Property<bool>("Lock");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("RoleID");

                    b.Property<string>("SingleFileSize");

                    b.Property<string>("TotalFileSize");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            ID = "23m454h5",
                            Approve = true,
                            Avatar = "/notebook/images/avatar.png",
                            CanUploadFile = false,
                            CreateDate = new DateTime(2019, 9, 29, 5, 56, 20, 918, DateTimeKind.Local).AddTicks(5343),
                            Email = "mhaf69@gmail.com",
                            LastActiveDate = new DateTime(2019, 9, 29, 5, 56, 20, 919, DateTimeKind.Local).AddTicks(3257),
                            Lock = false,
                            Name = "Muammer Hafızoğlu",
                            Password = "D3CE20FCCBE7D116ECD0",
                            Username = "muammer.hafizoglu"
                        });
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserGroup", b =>
                {
                    b.Property<string>("UserID");

                    b.Property<string>("GroupID");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("ID")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("Notification");

                    b.Property<int>("Status");

                    b.HasKey("UserID", "GroupID");

                    b.HasAlternateKey("ID");

                    b.HasIndex("GroupID");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserNote", b =>
                {
                    b.Property<string>("UserID");

                    b.Property<string>("NoteID");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("ID")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("Status");

                    b.HasKey("UserID", "NoteID");

                    b.HasAlternateKey("ID");

                    b.HasIndex("NoteID");

                    b.ToTable("UserNote");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserSettings", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(8);

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Calendar", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.User", "User")
                        .WithMany("Calendars")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Event", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Folder", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.Group", "Group")
                        .WithMany("Folders")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Follow", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notebook.Entities.Entities.User", "Following")
                        .WithMany("Follower")
                        .HasForeignKey("FollowingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.Note", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.Folder", "Folder")
                        .WithMany("Notes")
                        .HasForeignKey("FolderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notebook.Entities.Entities.Group", "Group")
                        .WithMany("Notes")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.User", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID");
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserGroup", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notebook.Entities.Entities.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserNote", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.Note", "Note")
                        .WithMany("Users")
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Notebook.Entities.Entities.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Notebook.Entities.Entities.UserSettings", b =>
                {
                    b.HasOne("Notebook.Entities.Entities.User", "User")
                        .WithOne("Settings")
                        .HasForeignKey("Notebook.Entities.Entities.UserSettings", "UserID");
                });
#pragma warning restore 612, 618
        }
    }
}