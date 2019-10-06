using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notebook.DataAccess.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Calendar",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "Calendar",
                newName: "Finish");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 29, 17, 40, 22, 550, DateTimeKind.Local).AddTicks(1834), new DateTime(2019, 9, 29, 17, 40, 22, 550, DateTimeKind.Local).AddTicks(9666) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Calendar",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Finish",
                table: "Calendar",
                newName: "FinishDate");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "ID",
                keyValue: "23m454h5",
                columns: new[] { "CreateDate", "LastActiveDate" },
                values: new object[] { new DateTime(2019, 9, 29, 5, 56, 20, 918, DateTimeKind.Local).AddTicks(5343), new DateTime(2019, 9, 29, 5, 56, 20, 919, DateTimeKind.Local).AddTicks(3257) });
        }
    }
}
