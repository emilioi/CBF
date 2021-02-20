using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class emailtemplatess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Email_Templates",
                maxLength: 225,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Email_Templates",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "FromAddress",
                table: "Email_Templates",
                maxLength: 225,
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EmailID",
                table: "Email_Templates",
                
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Email_Templates",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 225,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Email_Templates",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromAddress",
                table: "Email_Templates",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 225,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailID",
                table: "Email_Templates",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15);
        }
    }
}
