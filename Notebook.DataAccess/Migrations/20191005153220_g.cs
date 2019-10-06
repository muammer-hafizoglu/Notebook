using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedFileTypes",
                table: "Settings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CanUploadFile", "CreateDate", "LastActiveDate", "SingleFileSize", "TotalFileSize" },
                values: new object[] { true, new DateTime(2019, 10, 5, 18, 32, 20, 20, DateTimeKind.Local).AddTicks(8742), new DateTime(2019, 10, 5, 18, 32, 20, 22, DateTimeKind.Local).AddTicks(5508), "9999999999999", "999999999999999" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedFileTypes",
                table: "Settings");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CanUploadFile", "CreateDate", "LastActiveDate", "SingleFileSize", "TotalFileSize" },
                values: new object[] { false, new DateTime(2019, 9, 30, 13, 57, 20, 781, DateTimeKind.Local).AddTicks(9978), new DateTime(2019, 9, 30, 13, 57, 20, 785, DateTimeKind.Local).AddTicks(5378), null, null });
        }
    }
}
