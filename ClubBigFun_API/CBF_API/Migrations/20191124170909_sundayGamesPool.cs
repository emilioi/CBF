using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class sundayGamesPool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SundayGames",
                table: "Pool_Master",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.CreateTable(
            //    name: "NHLSchedule",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false),
            //        Week = table.Column<string>(nullable: true),
            //        StartTime = table.Column<string>(nullable: true),
            //        EndedTime = table.Column<string>(nullable: true),
            //        VenueAllegiance = table.Column<string>(nullable: true),
            //        ScheduleStatus = table.Column<string>(nullable: true),
            //        OriginalStartTime = table.Column<string>(nullable: true),
            //        DelayedOrPostponedReason = table.Column<string>(nullable: true),
            //        PlayedStatus = table.Column<string>(nullable: true),
            //        Attendance = table.Column<string>(nullable: true),
            //        Officials = table.Column<string>(nullable: true),
            //        Broadcasters = table.Column<string>(nullable: true),
            //        Weather = table.Column<string>(nullable: true),
            //        HomeTeamId = table.Column<int>(nullable: false),
            //        VisitingTeamID = table.Column<int>(nullable: false),
            //        HomeTeamShort = table.Column<string>(nullable: true),
            //        VisitingTeamShort = table.Column<string>(nullable: true),
            //        GameDate = table.Column<DateTime>(nullable: false),
            //        CutOffDate = table.Column<DateTime>(nullable: true),
            //        Venue_ID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NHLSchedule", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "NHLScore",
            //    columns: table => new
            //    {
            //        CurrentQuarter = table.Column<string>(nullable: true),
            //        CurrentQuarterSecondsRemaining = table.Column<string>(nullable: true),
            //        CurrentIntermission = table.Column<string>(nullable: true),
            //        TeamInPossession = table.Column<string>(nullable: true),
            //        CurrentDown = table.Column<string>(nullable: true),
            //        CurrentYardsRemaining = table.Column<string>(nullable: true),
            //        LineOfScrimmage = table.Column<string>(nullable: true),
            //        AwayScoreTotal = table.Column<string>(nullable: true),
            //        HomeScoreTotal = table.Column<string>(nullable: true),
            //        Quarters = table.Column<string>(nullable: true),
            //        Schedule_Id = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NHLScore", x => x.Schedule_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "NHLTeam",
            //    columns: table => new
            //    {
            //        Team_Id = table.Column<int>(nullable: false),
            //        Abbreviation = table.Column<string>(nullable: true),
            //        SportType = table.Column<string>(nullable: true),
            //        City = table.Column<string>(nullable: true),
            //        Team_Name = table.Column<string>(nullable: true),
            //        Venue_ID = table.Column<string>(nullable: true),
            //        LogoImageSrc = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NHLTeam", x => x.Team_Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "NHLVenue",
            //    columns: table => new
            //    {
            //        Venue_ID = table.Column<int>(nullable: false),
            //        Venue_Name = table.Column<string>(nullable: true),
            //        City = table.Column<string>(nullable: true),
            //        Country = table.Column<string>(nullable: true),
            //        GeoCoordinates = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NHLVenue", x => x.Venue_ID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NHLSchedule");

            migrationBuilder.DropTable(
                name: "NHLScore");

            migrationBuilder.DropTable(
                name: "NHLTeam");

            migrationBuilder.DropTable(
                name: "NHLVenue");

            migrationBuilder.DropColumn(
                name: "SundayGames",
                table: "Pool_Master");
        }
    }
}
