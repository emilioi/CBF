using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image_Name",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NFLScheduleList",
                columns: table => new
                {
                    Week = table.Column<string>(nullable: false),
                    GameDate = table.Column<DateTime>(nullable: false),
                    CutOffDate = table.Column<DateTime>(nullable: false),
                    Venue_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFLScheduleList", x => new { x.Week, x.GameDate, x.CutOffDate });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFLScheduleList");

            migrationBuilder.DropColumn(
                name: "Image_Name",
                table: "Members");
        }
    }
}
