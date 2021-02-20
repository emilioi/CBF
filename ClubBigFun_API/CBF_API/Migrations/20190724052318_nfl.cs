using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class nfl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeTeamLogo",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamName",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamSortName",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VenueTeamLogo",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitingTeamName",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitingTeamSortName",
                table: "NFLScheduleList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image_Name",
                table: "Administrators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeTeamLogo",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "HomeTeamName",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "HomeTeamSortName",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "VenueTeamLogo",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "VisitingTeamName",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "VisitingTeamSortName",
                table: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "Image_Name",
                table: "Administrators");
        }
    }
}
