using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Follow",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 9, 18, 43, 38, 53, DateTimeKind.Local).AddTicks(1538), new DateTime(2019, 9, 9, 18, 43, 38, 55, DateTimeKind.Local).AddTicks(3871) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Follow");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 2, 8, 32, 11, 541, DateTimeKind.Local).AddTicks(2580), new DateTime(2019, 9, 2, 8, 32, 11, 544, DateTimeKind.Local).AddTicks(779) });
        }
    }
}
