using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class rulechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "GameType",
                table: "Rules");

            migrationBuilder.AddColumn<int>(
                name: "Rule_Id",
                table: "Rules",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Game_Type",
                table: "Rules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rule_Title",
                table: "Rules",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "Rule_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Rule_Id",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Game_Type",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "Rule_Title",
                table: "Rules");

            migrationBuilder.AddColumn<string>(
                name: "GameType",
                table: "Rules",
                maxLength: 55,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "GameType");
        }
    }
}
