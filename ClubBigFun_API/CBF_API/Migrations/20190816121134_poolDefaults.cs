using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class poolDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pool_Defaults",
                table: "Pool_Defaults");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Pool_Defaults_Pool_Id_Schedule_Id_Team_Id_WeekNumber",
                table: "Pool_Defaults",
                columns: new[] { "Pool_Id", "Schedule_Id", "Team_Id", "WeekNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pool_Defaults",
                table: "Pool_Defaults",
                columns: new[] { "Pool_Id", "WeekNumber", "Schedule_Id", "Team_Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Pool_Defaults_Pool_Id_Schedule_Id_Team_Id_WeekNumber",
                table: "Pool_Defaults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pool_Defaults",
                table: "Pool_Defaults");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pool_Defaults",
                table: "Pool_Defaults",
                column: "Id");
        }
    }
}
