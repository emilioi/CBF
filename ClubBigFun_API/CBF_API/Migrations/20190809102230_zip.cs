using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class zip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Home_Phone",
                table: "Members",
                newName: "Zip_Code");

            migrationBuilder.AddColumn<string>(
                name: "Business_Phone",
                table: "Members",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Business_Phone",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Zip_Code",
                table: "Members",
                newName: "Home_Phone");
        }
    }
}
