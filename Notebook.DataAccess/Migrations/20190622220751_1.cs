using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "NotebookSettings",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Authorization = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
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
                name: "Folder",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    Visible = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Folder_User_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FollowerID = table.Column<string>(nullable: false),
                    FollowingID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Notification = table.Column<bool>(nullable: false)
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
                name: "Group",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Explanation = table.Column<string>(nullable: true),
                    Visible = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsRequiredApproval = table.Column<bool>(nullable: false),
                    OwnerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Group_User_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "User",
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
                    OwnerID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Note_User_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "UserFolder",
                columns: table => new
                {
                    FolderID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Notification = table.Column<bool>(nullable: false),
                    MemberType = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFolder", x => new { x.UserID, x.FolderID });
                    table.UniqueConstraint("AK_UserFolder_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserFolder_Folder_FolderID",
                        column: x => x.FolderID,
                        principalTable: "Folder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFolder_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupFolder",
                columns: table => new
                {
                    FolderID = table.Column<string>(nullable: false),
                    GroupID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupFolder", x => new { x.GroupID, x.FolderID });
                    table.UniqueConstraint("AK_GroupFolder_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GroupFolder_Folder_FolderID",
                        column: x => x.FolderID,
                        principalTable: "Folder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupFolder_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
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
                    MemberType = table.Column<int>(nullable: false),
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
                name: "FolderNote",
                columns: table => new
                {
                    NoteID = table.Column<string>(nullable: false),
                    FolderID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderNote", x => new { x.FolderID, x.NoteID });
                    table.UniqueConstraint("AK_FolderNote_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FolderNote_Folder_FolderID",
                        column: x => x.FolderID,
                        principalTable: "Folder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderNote_Note_NoteID",
                        column: x => x.NoteID,
                        principalTable: "Note",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupNote",
                columns: table => new
                {
                    NoteID = table.Column<string>(nullable: false),
                    GroupID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupNote", x => new { x.GroupID, x.NoteID });
                    table.UniqueConstraint("AK_GroupNote_ID", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GroupNote_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupNote_Note_NoteID",
                        column: x => x.NoteID,
                        principalTable: "Note",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNote",
                columns: table => new
                {
                    NoteID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Member = table.Column<int>(nullable: false)
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
                columns: new[] { "ID", "Authorization", "Name" },
                values: new object[] { "863a12r5", "ADD_ROLE,EDIT_ROLE,NEW_ROLE,ADD_USER,EDIT_USER,NEW_USER,USER_ROLE", "admin" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "ID", "Approve", "Avatar", "CreateDate", "Email", "Info", "LastActiveDate", "Name", "Password", "RoleID", "Username" },
                values: new object[] { "23m454h5", true, "/notebook/images/avatar.png", new DateTime(2019, 6, 23, 1, 7, 51, 464, DateTimeKind.Local).AddTicks(1211), "muammer.hafizogluu@gmail.com", null, new DateTime(2019, 6, 23, 1, 7, 51, 465, DateTimeKind.Local).AddTicks(1281), "Muammer Hafızoğlu", "D3CE20FCCBE7D116ECD0", null, "muammer.hafizoglu" });

            migrationBuilder.CreateIndex(
                name: "IX_Folder_OwnerID",
                table: "Folder",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_FolderNote_NoteID",
                table: "FolderNote",
                column: "NoteID");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowingID",
                table: "Follow",
                column: "FollowingID");

            migrationBuilder.CreateIndex(
                name: "IX_Group_OwnerID",
                table: "Group",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupFolder_FolderID",
                table: "GroupFolder",
                column: "FolderID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupNote_NoteID",
                table: "GroupNote",
                column: "NoteID");

            migrationBuilder.CreateIndex(
                name: "IX_Note_OwnerID",
                table: "Note",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserFolder_FolderID",
                table: "UserFolder",
                column: "FolderID");

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
                name: "FolderNote");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "GroupFolder");

            migrationBuilder.DropTable(
                name: "GroupNote");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "NotebookSettings");

            migrationBuilder.DropTable(
                name: "UserFolder");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserNote");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
