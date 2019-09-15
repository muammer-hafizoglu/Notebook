using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    Visible = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsRequiredApproval = table.Column<bool>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Detail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    IsModule = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    WebAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    DefaultLanguage = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Port = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsMailActive = table.Column<bool>(nullable: false),
                    Gmail = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Linkedin = table.Column<string>(nullable: true),
                    IsMembershipOpen = table.Column<bool>(nullable: false),
                    MembershipEmailControl = table.Column<bool>(nullable: false),
                    TotalFileSize = table.Column<string>(nullable: true),
                    SingleFileSize = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    Visible = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    GroupID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Folder_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Approve = table.Column<bool>(nullable: false),
                    Lock = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastActiveDate = table.Column<DateTime>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    RoleID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Visible = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ReadCount = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    OpenToCopy = table.Column<bool>(nullable: false),
                    OpenToComments = table.Column<bool>(nullable: false),
                    CopyCount = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    GroupID = table.Column<string>(nullable: true),
                    FolderID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Note_Folder_FolderID",
                        column: x => x.FolderID,
                        principalTable: "Folder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FollowerID = table.Column<string>(nullable: false),
                    FollowingID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Notification = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => new { x.FollowerID, x.FollowingID });
                    table.UniqueConstraint("AK_Follow_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Follow_User_FollowerID",
                        column: x => x.FollowerID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follow_User_FollowingID",
                        column: x => x.FollowingID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    GroupID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Notification = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.UserID, x.GroupID });
                    table.UniqueConstraint("AK_UserGroup_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSettings_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNote",
                columns: table => new
                {
                    NoteID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNote", x => new { x.UserID, x.NoteID });
                    table.UniqueConstraint("AK_UserNote_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserNote_Note_NoteID",
                        column: x => x.NoteID,
                        principalTable: "Note",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNote_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "ID", "Name", "Permissions" },
                values: new object[] { "56854644", "Admin", "VIEW_ADMINPANEL,VIEW_ROLE,ADD_ROLE,EDIT_ROLE,DELETE_ROLE,EDIT_SETTINGS,VIEW_USERS,EDIT_USERS,DELETE_USERS" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "ID", "Address", "DefaultLanguage", "Description", "Email", "Facebook", "Footer", "Gmail", "Host", "Icon", "Introduction", "IsMailActive", "IsMembershipOpen", "Linkedin", "Logo", "MembershipEmailControl", "Metadata", "Name", "Password", "Phone", "Port", "SingleFileSize", "Title", "TotalFileSize", "Twitter", "Username", "WebAddress" },
                values: new object[] { "45634df5", null, "en-EN", null, null, null, "Copyright © 2014-2016 Almsaeed Studio. All rights reserved. ", null, null, "/favicon.ico", null, false, true, null, "/notebook/images/logo.png", false, null, null, null, null, null, null, "Notebook", null, null, null, "https://www.notebook.com" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "ID", "Approve", "Avatar", "CreateDate", "Email", "Info", "LastActiveDate", "Lock", "Name", "Password", "RoleID", "Username" },
                values: new object[] { "23m454h5", true, "/notebook/images/avatar.png", new DateTime(2019, 9, 2, 8, 32, 11, 541, DateTimeKind.Local).AddTicks(2580), "mhaf69@gmail.com", null, new DateTime(2019, 9, 2, 8, 32, 11, 544, DateTimeKind.Local).AddTicks(779), false, "Muammer Hafızoğlu", "D3CE20FCCBE7D116ECD0", null, "muammer.hafizoglu" });

            migrationBuilder.CreateIndex(
                name: "IX_Folder_GroupID",
                table: "Folder",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowingID",
                table: "Follow",
                column: "FollowingID");

            migrationBuilder.CreateIndex(
                name: "IX_Note_FolderID",
                table: "Note",
                column: "FolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Note_GroupID",
                table: "Note",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupID",
                table: "UserGroup",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserNote_NoteID",
                table: "UserNote",
                column: "NoteID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserID",
                table: "UserSettings",
                column: "UserID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserNote");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
