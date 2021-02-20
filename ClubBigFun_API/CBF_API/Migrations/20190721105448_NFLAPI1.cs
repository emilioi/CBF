using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class NFLAPI1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "week",
                table: "NFLSchedules",
                newName: "Week");

            migrationBuilder.RenameColumn(
                name: "weather",
                table: "NFLSchedules",
                newName: "Weather");

            migrationBuilder.RenameColumn(
                name: "venueAllegiance",
                table: "NFLSchedules",
                newName: "VenueAllegiance");

            migrationBuilder.RenameColumn(
                name: "startTime",
                table: "NFLSchedules",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "scheduleStatus",
                table: "NFLSchedules",
                newName: "ScheduleStatus");

            migrationBuilder.RenameColumn(
                name: "playedStatus",
                table: "NFLSchedules",
                newName: "PlayedStatus");

            migrationBuilder.RenameColumn(
                name: "originalStartTime",
                table: "NFLSchedules",
                newName: "OriginalStartTime");

            migrationBuilder.RenameColumn(
                name: "officials",
                table: "NFLSchedules",
                newName: "Officials");

            migrationBuilder.RenameColumn(
                name: "endedTime",
                table: "NFLSchedules",
                newName: "EndedTime");

            migrationBuilder.RenameColumn(
                name: "delayedOrPostponedReason",
                table: "NFLSchedules",
                newName: "DelayedOrPostponedReason");

            migrationBuilder.RenameColumn(
                name: "broadcasters",
                table: "NFLSchedules",
                newName: "Broadcasters");

            migrationBuilder.RenameColumn(
                name: "attendance",
                table: "NFLSchedules",
                newName: "Attendance");

            migrationBuilder.AlterColumn<string>(
                name: "TeamNameShort",
                table: "Team_List",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Team_List",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TeamLogo",
                table: "Team_List",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeName",
                table: "Sports_Type",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Rules_URL",
                table: "Pool_Master",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pool_Name",
                table: "Pool_Master",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CutOffDate",
                table: "NFLSchedules",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GameDate",
                table: "NFLSchedules",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamId",
                table: "NFLSchedules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamShort",
                table: "NFLSchedules",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Venue_ID",
                table: "NFLSchedules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisitingTeamID",
                table: "NFLSchedules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitingTeamShort",
                table: "NFLSchedules",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "MailingList",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Template_Subject",
                table: "Email_Templates",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Template_For",
                table: "Email_Templates",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Template_Description",
                table: "Email_Templates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CutOffDate",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "GameDate",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "HomeTeamId",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "HomeTeamShort",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "Venue_ID",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "VisitingTeamID",
                table: "NFLSchedules");

            migrationBuilder.DropColumn(
                name: "VisitingTeamShort",
                table: "NFLSchedules");

            migrationBuilder.RenameColumn(
                name: "Week",
                table: "NFLSchedules",
                newName: "week");

            migrationBuilder.RenameColumn(
                name: "Weather",
                table: "NFLSchedules",
                newName: "weather");

            migrationBuilder.RenameColumn(
                name: "VenueAllegiance",
                table: "NFLSchedules",
                newName: "venueAllegiance");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "NFLSchedules",
                newName: "startTime");

            migrationBuilder.RenameColumn(
                name: "ScheduleStatus",
                table: "NFLSchedules",
                newName: "scheduleStatus");

            migrationBuilder.RenameColumn(
                name: "PlayedStatus",
                table: "NFLSchedules",
                newName: "playedStatus");

            migrationBuilder.RenameColumn(
                name: "OriginalStartTime",
                table: "NFLSchedules",
                newName: "originalStartTime");

            migrationBuilder.RenameColumn(
                name: "Officials",
                table: "NFLSchedules",
                newName: "officials");

            migrationBuilder.RenameColumn(
                name: "EndedTime",
                table: "NFLSchedules",
                newName: "endedTime");

            migrationBuilder.RenameColumn(
                name: "DelayedOrPostponedReason",
                table: "NFLSchedules",
                newName: "delayedOrPostponedReason");

            migrationBuilder.RenameColumn(
                name: "Broadcasters",
                table: "NFLSchedules",
                newName: "broadcasters");

            migrationBuilder.RenameColumn(
                name: "Attendance",
                table: "NFLSchedules",
                newName: "attendance");

            migrationBuilder.AlterColumn<string>(
                name: "TeamNameShort",
                table: "Team_List",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TeamName",
                table: "Team_List",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TeamLogo",
                table: "Team_List",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeName",
                table: "Sports_Type",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Rules_URL",
                table: "Pool_Master",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Pool_Name",
                table: "Pool_Master",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "MailingList",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Template_Subject",
                table: "Email_Templates",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Template_For",
                table: "Email_Templates",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Template_Description",
                table: "Email_Templates",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
