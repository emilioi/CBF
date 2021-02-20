using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class Encryption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Is_Temp_Password",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Locked",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Email_Verified",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Deleted",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<bool>(
                name: "Is_Active",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    TokenID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    SessionGUID = table.Column<string>(nullable: true),
                    UserHostIPAddress = table.Column<string>(nullable: true),
                    RequestBrowsertypeVersion = table.Column<string>(nullable: true),
                    BrowserUniqueID = table.Column<string>(nullable: true),
                    AppDomainName = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    IssuedOn = table.Column<DateTime>(nullable: true),
                    Expired = table.Column<bool>(nullable: false),
                    ExpiredOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => x.TokenID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.AlterColumn<byte>(
                name: "Is_Temp_Password",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<byte>(
                name: "Is_Locked",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<byte>(
                name: "Is_Email_Verified",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<byte>(
                name: "Is_Deleted",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<byte>(
                name: "Is_Active",
                table: "Users",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
