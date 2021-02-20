using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class activepool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Private",
                table: "Pool_Master",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThrusdaySaturdayGames",
                table: "Pool_Master",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Private",
                table: "Pool_Master");

            migrationBuilder.DropColumn(
                name: "ThrusdaySaturdayGames",
                table: "Pool_Master");
        }
    }
}
