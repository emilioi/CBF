using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class weekNumberInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "WeekNumber",
            //    table: "survPoolWeekList",
            //    nullable: false,
            //    oldClrType: typeof(short));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Sent_On",
                table: "Email_Notification",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "WeekNumber",
                table: "survPoolWeekList",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Sent_On",
                table: "Email_Notification",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
