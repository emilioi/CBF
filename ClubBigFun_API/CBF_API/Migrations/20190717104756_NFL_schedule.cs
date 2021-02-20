using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class NFL_schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFL_Schedule",
                columns: table => new
                {
                    WeekNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HomeTeamShort = table.Column<string>(maxLength: 50, nullable: true),
                    VisitingTeamShort = table.Column<string>(maxLength: 50, nullable: true),
                    HomeTeam = table.Column<string>(maxLength: 50, nullable: true),
                    VisitingTeam = table.Column<string>(maxLength: 50, nullable: true),
                    HomeTeamID = table.Column<int>(nullable: false),
                    VisitingTeamID = table.Column<int>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    GameDate = table.Column<DateTime>(nullable: false),
                    CutOffDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFL_Schedule", x => x.WeekNumber);
                });

            migrationBuilder.CreateTable(
                name: "Team_List",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SportType = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(maxLength: 50, nullable: true),
                    TeamNameShort = table.Column<string>(maxLength: 50, nullable: true),
                    TeamLogo = table.Column<string>(maxLength: 50, nullable: true),
                    Is_Deleted = table.Column<bool>(nullable: false),
                    Last_Edited_By = table.Column<int>(nullable: false),
                    DTS = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_List", x => x.TeamID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFL_Schedule");

            migrationBuilder.DropTable(
                name: "Team_List");
        }
    }
}
