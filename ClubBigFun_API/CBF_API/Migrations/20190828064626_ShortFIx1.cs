using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class ShortFIx1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Winner",
                table: "SurvScheduleList",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Winner",
                table: "SurvScheduleList",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
