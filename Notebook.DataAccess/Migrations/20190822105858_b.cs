using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFolder");

            migrationBuilder.DropColumn(
                name: "Member",
                table: "UserNote");

            migrationBuilder.DropColumn(
                name: "MemberType",
                table: "UserGroup");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserNote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 8, 22, 13, 58, 58, 123, DateTimeKind.Local).AddTicks(4544), new DateTime(2019, 8, 22, 13, 58, 58, 126, DateTimeKind.Local).AddTicks(5991) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserNote");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserGroup");

            migrationBuilder.AddColumn<int>(
                name: "Member",
                table: "UserNote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberType",
                table: "UserGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserFolder",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    FolderID = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    MemberType = table.Column<int>(nullable: false),
                    Notification = table.Column<bool>(nullable: false)
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

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 8, 14, 14, 41, 26, 267, DateTimeKind.Local).AddTicks(5429), new DateTime(2019, 8, 14, 14, 41, 26, 268, DateTimeKind.Local).AddTicks(3186) });

            migrationBuilder.CreateIndex(
                name: "IX_UserFolder_FolderID",
                table: "UserFolder",
                column: "FolderID");
        }
    }
}
