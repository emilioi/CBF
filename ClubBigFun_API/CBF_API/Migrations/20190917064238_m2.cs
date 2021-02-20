using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Prefix",
            //    table: "SurvEntries");

            migrationBuilder.RenameColumn(
                name: "Next_Reminder",
                table: "Member_Alerts",
                newName: "ReminderStart");

            migrationBuilder.RenameColumn(
                name: "Last_Reminder",
                table: "Member_Alerts",
                newName: "ReminderEnd");

            migrationBuilder.AddColumn<string>(
                name: "Alert_Description",
                table: "Member_Alerts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alert_Description",
                table: "Member_Alerts");

            migrationBuilder.RenameColumn(
                name: "ReminderStart",
                table: "Member_Alerts",
                newName: "Next_Reminder");

            migrationBuilder.RenameColumn(
                name: "ReminderEnd",
                table: "Member_Alerts",
                newName: "Last_Reminder");

            //migrationBuilder.AddColumn<string>(
            //    name: "Prefix",
            //    table: "SurvEntries",
            //    maxLength: 50,
            //    nullable: true);
        }
    }
}
