using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class imagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLScores",
                table: "NFLScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLSchedules",
                table: "NFLSchedules");

            migrationBuilder.RenameTable(
                name: "NFLScores",
                newName: "NFLScore");

            migrationBuilder.RenameTable(
                name: "NFLSchedules",
                newName: "NFLSchedule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore",
                column: "Score_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLSchedule",
                table: "NFLSchedule",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLSchedule",
                table: "NFLSchedule");

            migrationBuilder.RenameTable(
                name: "NFLScore",
                newName: "NFLScores");

            migrationBuilder.RenameTable(
                name: "NFLSchedule",
                newName: "NFLSchedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLScores",
                table: "NFLScores",
                column: "Score_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLSchedules",
                table: "NFLSchedules",
                column: "Id");
        }
    }
}
