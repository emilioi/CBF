using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class NFLAPI12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFLScores",
                columns: table => new
                {
                    Score_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentQuarter = table.Column<string>(nullable: true),
                    CurrentQuarterSecondsRemaining = table.Column<string>(nullable: true),
                    CurrentIntermission = table.Column<string>(nullable: true),
                    TeamInPossession = table.Column<string>(nullable: true),
                    CurrentDown = table.Column<string>(nullable: true),
                    CurrentYardsRemaining = table.Column<string>(nullable: true),
                    LineOfScrimmage = table.Column<string>(nullable: true),
                    AwayScoreTotal = table.Column<string>(nullable: true),
                    HomeScoreTotal = table.Column<string>(nullable: true),
                    Quarters = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLScores", x => x.Score_Id);
                });

            migrationBuilder.CreateTable(
                name: "NFLTeam",
                columns: table => new
                {
                    Team_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abbreviation = table.Column<string>(nullable: true),
                    SportType = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Team_Name = table.Column<string>(nullable: true),
                    Venue_ID = table.Column<string>(nullable: true),
                    LogoImageSrc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLTeam", x => x.Team_Id);
                });

            migrationBuilder.CreateTable(
                name: "NFLVenue",
                columns: table => new
                {
                    Venue_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Venue_Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    GeoCoordinates = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLVenue", x => x.Venue_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFLScores");

            migrationBuilder.DropTable(
                name: "NFLTeam");

            migrationBuilder.DropTable(
                name: "NFLVenue");
        }
    }
}
