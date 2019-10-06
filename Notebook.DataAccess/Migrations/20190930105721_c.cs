using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 8, nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsView = table.Column<bool>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 30, 13, 57, 20, 781, DateTimeKind.Local).AddTicks(9978), new DateTime(2019, 9, 30, 13, 57, 20, 785, DateTimeKind.Local).AddTicks(5378) });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 29, 17, 40, 22, 550, DateTimeKind.Local).AddTicks(1834), new DateTime(2019, 9, 29, 17, 40, 22, 550, DateTimeKind.Local).AddTicks(9666) });
        }
    }
}
