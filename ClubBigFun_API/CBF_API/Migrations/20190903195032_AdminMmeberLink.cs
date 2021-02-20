using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class AdminMmeberLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberAdminLink",
                columns: table => new
                {
                    Admin_ID = table.Column<int>(nullable: false),
                    Member_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAdminLink", x => new { x.Member_ID, x.Admin_ID });
                    table.UniqueConstraint("AK_MemberAdminLink_Admin_ID_Member_ID", x => new { x.Admin_ID, x.Member_ID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberAdminLink");
        }
    }
}
