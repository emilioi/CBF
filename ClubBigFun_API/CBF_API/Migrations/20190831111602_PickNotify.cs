using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class PickNotify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultedNotificationSent",
                table: "SurvEntryPicks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EliminatedNotificationSent",
                table: "SurvEntryPicks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WinnerNotificationSent",
                table: "SurvEntryPicks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultedNotificationSent",
                table: "SurvEntryPicks");

            migrationBuilder.DropColumn(
                name: "EliminatedNotificationSent",
                table: "SurvEntryPicks");

            migrationBuilder.DropColumn(
                name: "WinnerNotificationSent",
                table: "SurvEntryPicks");
        }
    }
}
