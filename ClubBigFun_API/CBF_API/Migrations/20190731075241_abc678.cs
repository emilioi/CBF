using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class abc678 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Clubs");

            //migrationBuilder.DropTable(
            //    name: "EntryMenus");

            //migrationBuilder.DropTable(
            //    name: "NFLScheduleList");

            //migrationBuilder.DropTable(
            //    name: "");

            //migrationBuilder.DropTable(
            //    name: "qrySurvEntries");

            //migrationBuilder.DropTable(
            //    name: "qrySurvScheduleList");

            //migrationBuilder.DropTable(
            //    name: "ScheduleMenus");

            //migrationBuilder.DropTable(
            //    name: "SurvPoolWeekListMapped");

            //migrationBuilder.DropTable(
            //    name: "WeekMenu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Pool_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cut_Off = table.Column<DateTime>(nullable: false),
                    DTS = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Is_Active = table.Column<bool>(nullable: false),
                    Is_Deleted = table.Column<bool>(nullable: false),
                    Is_Started = table.Column<bool>(nullable: false),
                    Last_Edited_By = table.Column<int>(nullable: false),
                    Pool_Name = table.Column<string>(maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Ruler_Season = table.Column<bool>(nullable: false),
                    Rules_URL = table.Column<string>(maxLength: 250, nullable: true),
                    Sport_Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Pool_ID);
                });

            migrationBuilder.CreateTable(
                name: "EntryMenus",
                columns: table => new
                {
                    Pool_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pool_Name = table.Column<string>(nullable: true),
                    TheCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryMenus", x => x.Pool_ID);
                });

            migrationBuilder.CreateTable(
                name: "NFLScheduleList",
                columns: table => new
                {
                    Week = table.Column<string>(nullable: false),
                    GameDate = table.Column<DateTime>(nullable: false),
                    CutOffDate = table.Column<DateTime>(nullable: false),
                    HomeTeamLogo = table.Column<string>(nullable: true),
                    HomeTeamName = table.Column<string>(nullable: true),
                    HomeTeamSortName = table.Column<string>(nullable: true),
                    VenueTeamLogo = table.Column<string>(nullable: true),
                    Venue_Name = table.Column<string>(nullable: true),
                    VisitingTeamName = table.Column<string>(nullable: true),
                    VisitingTeamSortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLScheduleList", x => new { x.Week, x.GameDate, x.CutOffDate });
                });

            migrationBuilder.CreateTable(
                name: "PicksCount",
                columns: table => new
                {
                    EntryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumOfPicks = table.Column<int>(nullable: false),
                    PoolID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PicksCount", x => x.EntryID);
                });

            migrationBuilder.CreateTable(
                name: "qrySurvEntries",
                columns: table => new
                {
                    PoolID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    Defaults = table.Column<int>(nullable: false),
                    Eliminated = table.Column<bool>(nullable: false),
                    EntryID = table.Column<int>(nullable: false),
                    EntryName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Login_ID = table.Column<string>(nullable: true),
                    MemberID = table.Column<int>(nullable: false),
                    Pool_Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrySurvEntries", x => x.PoolID);
                });

            migrationBuilder.CreateTable(
                name: "qrySurvScheduleList",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CutOff = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HomeLogoImageSrc = table.Column<string>(nullable: true),
                    HomeTeam = table.Column<int>(nullable: false),
                    HomeTeamName = table.Column<string>(nullable: true),
                    HomeTeamNameShort = table.Column<string>(nullable: true),
                    PoolID = table.Column<int>(nullable: false),
                    Pool_Name = table.Column<string>(nullable: true),
                    Start = table.Column<bool>(nullable: false),
                    VisitingLogoImageSrc = table.Column<string>(nullable: true),
                    VisitingTeam = table.Column<int>(nullable: false),
                    VisitingTeamName = table.Column<string>(nullable: true),
                    VisitingTeamNameShort = table.Column<string>(nullable: true),
                    WeekNumber = table.Column<short>(nullable: false),
                    Winner = table.Column<string>(nullable: true),
                    WinnerLogoImageSrc = table.Column<string>(nullable: true),
                    WinnerTeamName = table.Column<string>(nullable: true),
                    WinnerTeamNameShort = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrySurvScheduleList", x => x.ScheduleID);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleMenus",
                columns: table => new
                {
                    Pool_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pool_Name = table.Column<string>(nullable: true),
                    theCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleMenus", x => x.Pool_ID);
                });

            migrationBuilder.CreateTable(
                name: "SurvPoolWeekListMapped",
                columns: table => new
                {
                    PoolID = table.Column<int>(nullable: false),
                    WeekNumber = table.Column<short>(nullable: false),
                    CutOff = table.Column<DateTime>(nullable: true),
                    Pool_Name = table.Column<string>(nullable: true),
                    Start = table.Column<bool>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvPoolWeekListMapped", x => new { x.PoolID, x.WeekNumber });
                });

            migrationBuilder.CreateTable(
                name: "WeekMenu",
                columns: table => new
                {
                    Pool_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pool_Name = table.Column<string>(nullable: true),
                    WeekCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekMenu", x => x.Pool_ID);
                });
        }
    }
}
