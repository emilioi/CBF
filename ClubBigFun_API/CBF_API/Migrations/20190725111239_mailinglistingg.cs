using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class mailinglistingg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Is_Deleted",
                table: "Email_Templates",
                nullable: false,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Is_Deleted",
                table: "Email_Templates",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
