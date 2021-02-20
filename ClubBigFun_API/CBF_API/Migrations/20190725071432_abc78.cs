using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class abc78 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFL_Schedule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFL_Schedule",
                columns: table => new
                {
                    WeekNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CutOffDate = table.Column<DateTime>(nullable: false),
                    Day = table.Column<string>(nullable: true),
                    GameDate = table.Column<DateTime>(nullable: false),
                    HomeTeam = table.Column<string>(maxLength: 50, nullable: true),
                    HomeTeamID = table.Column<int>(nullable: false),
                    HomeTeamShort = table.Column<string>(maxLength: 50, nullable: true),
                    VisitingTeam = table.Column<string>(maxLength: 50, nullable: true),
                    VisitingTeamID = table.Column<int>(nullable: false),
                    VisitingTeamShort = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFL_Schedule", x => x.WeekNumber);
                });
        }
    }
}
