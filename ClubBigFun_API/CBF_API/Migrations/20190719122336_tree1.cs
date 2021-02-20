using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class tree1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.CreateTable(
                name: "NFLSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    week = table.Column<string>(nullable: true),
                    startTime = table.Column<string>(nullable: true),
                    endedTime = table.Column<string>(nullable: true),
                    venueAllegiance = table.Column<string>(nullable: true),
                    scheduleStatus = table.Column<string>(nullable: true),
                    originalStartTime = table.Column<string>(nullable: true),
                    delayedOrPostponedReason = table.Column<string>(nullable: true),
                    playedStatus = table.Column<string>(nullable: true),
                    attendance = table.Column<string>(nullable: true),
                    officials = table.Column<string>(nullable: true),
                    broadcasters = table.Column<string>(nullable: true),
                    weather = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLSchedules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFLSchedules");

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    attendance = table.Column<string>(nullable: true),
                    broadcasters = table.Column<string>(nullable: true),
                    delayedOrPostponedReason = table.Column<string>(nullable: true),
                    endedTime = table.Column<string>(nullable: true),
                    officials = table.Column<string>(nullable: true),
                    originalStartTime = table.Column<string>(nullable: true),
                    playedStatus = table.Column<string>(nullable: true),
                    scheduleStatus = table.Column<string>(nullable: true),
                    startTime = table.Column<string>(nullable: true),
                    venueAllegiance = table.Column<string>(nullable: true),
                    weather = table.Column<string>(nullable: true),
                    week = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });
        }
    }
}
