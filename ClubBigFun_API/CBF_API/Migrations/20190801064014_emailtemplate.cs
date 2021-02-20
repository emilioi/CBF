using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class emailtemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Email_Templates",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Template_Id",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "DTS",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Is_Deleted",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Last_Edited_By",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Template_For",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Template_Subject",
                table: "Email_Templates");

            migrationBuilder.RenameColumn(
                name: "Template_Description",
                table: "Email_Templates",
                newName: "FromAddress");

            migrationBuilder.AddColumn<string>(
                name: "EmailID",
                table: "Email_Templates",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bcc",
                table: "Email_Templates",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BodyFormat",
                table: "Email_Templates",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Cc",
                table: "Email_Templates",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Importance",
                table: "Email_Templates",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MailFormat",
                table: "Email_Templates",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Email_Templates",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Email_Templates",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Email_Templates",
                table: "Email_Templates",
                column: "EmailID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Email_Templates",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "EmailID",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Bcc",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "BodyFormat",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Cc",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Importance",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "MailFormat",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Email_Templates");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Email_Templates");

            migrationBuilder.RenameColumn(
                name: "FromAddress",
                table: "Email_Templates",
                newName: "Template_Description");

            migrationBuilder.AddColumn<int>(
                name: "Template_Id",
                table: "Email_Templates",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DTS",
                table: "Email_Templates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_Deleted",
                table: "Email_Templates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Last_Edited_By",
                table: "Email_Templates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Template_For",
                table: "Email_Templates",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Template_Subject",
                table: "Email_Templates",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Email_Templates",
                table: "Email_Templates",
                column: "Template_Id");
        }
    }
}
