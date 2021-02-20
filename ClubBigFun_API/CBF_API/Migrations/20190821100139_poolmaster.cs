using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class poolmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassCode",
                table: "Pool_Master",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Purpose",
            //    table: "Email_Templates",
            //    maxLength: 250,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "EmailID",
            //    table: "Email_Templates",
            //    maxLength: 55,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 15);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassCode",
                table: "Pool_Master");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Email_Templates",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailID",
                table: "Email_Templates",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 55);
        }
    }
}
