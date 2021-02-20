using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class scoreAdded12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentQuarter",
                table: "NFLScore",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Schedule_Id",
                table: "NFLScore",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore",
                column: "Schedule_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore");

            migrationBuilder.DropColumn(
                name: "Schedule_Id",
                table: "NFLScore");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentQuarter",
                table: "NFLScore",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NFLScore",
                table: "NFLScore",
                column: "CurrentQuarter");
        }
    }
}
