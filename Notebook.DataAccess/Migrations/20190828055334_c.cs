using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 8, 28, 8, 53, 33, 724, DateTimeKind.Local).AddTicks(6516), new DateTime(2019, 8, 28, 8, 53, 33, 728, DateTimeKind.Local).AddTicks(1661) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lock",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 8, 22, 13, 58, 58, 123, DateTimeKind.Local).AddTicks(4544), new DateTime(2019, 8, 22, 13, 58, 58, 126, DateTimeKind.Local).AddTicks(5991) });
        }
    }
}
