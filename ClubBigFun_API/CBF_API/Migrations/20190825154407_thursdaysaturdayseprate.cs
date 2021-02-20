using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class thursdaysaturdayseprate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThrusdaySaturdayGames",
                table: "Pool_Master",
                newName: "ThrusdayGames");

            migrationBuilder.AddColumn<bool>(
                name: "SaturdayGames",
                table: "Pool_Master",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaturdayGames",
                table: "Pool_Master");

            migrationBuilder.RenameColumn(
                name: "ThrusdayGames",
                table: "Pool_Master",
                newName: "ThrusdaySaturdayGames");
        }
    }
}
