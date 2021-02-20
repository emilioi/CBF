using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Member_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login_Id = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    First_Name = table.Column<string>(maxLength: 100, nullable: true),
                    Last_Name = table.Column<string>(maxLength: 100, nullable: true),
                    Phone_Number = table.Column<string>(maxLength: 40, nullable: true),
                    Admin_Type = table.Column<string>(maxLength: 100, nullable: true),
                    Email_Address = table.Column<string>(maxLength: 100, nullable: true),
                    Image_Url = table.Column<string>(nullable: true),
                    Last_Login = table.Column<DateTime>(nullable: false),
                    Failed_Attempt = table.Column<int>(nullable: false),
                    Last_Failed_Login = table.Column<DateTime>(nullable: false),
                    Is_Email_Verified = table.Column<bool>(nullable: false),
                    Is_Temp_Password = table.Column<bool>(nullable: false),
                    Is_Locked = table.Column<bool>(nullable: false),
                    Is_Active = table.Column<bool>(nullable: false),
                    Is_Deleted = table.Column<bool>(nullable: false),
                    Last_Edited_By = table.Column<int>(nullable: false),
                    DTS = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Member_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");
        }
    }
}
