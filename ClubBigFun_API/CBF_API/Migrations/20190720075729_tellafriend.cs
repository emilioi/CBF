using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class tellafriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MailingLists",
                table: "MailingLists");

            migrationBuilder.RenameTable(
                name: "MailingLists",
                newName: "MailingList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MailingList",
                table: "MailingList",
                column: "MailingList_ID");

            migrationBuilder.CreateTable(
                name: "TellAFriend",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendName = table.Column<string>(maxLength: 256, nullable: true),
                    FriendEmail = table.Column<string>(maxLength: 256, nullable: true),
                    YourName = table.Column<string>(maxLength: 256, nullable: true),
                    YourEmail = table.Column<string>(maxLength: 256, nullable: true),
                    Referer = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDT = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TellAFriend", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TellAFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MailingList",
                table: "MailingList");

            migrationBuilder.RenameTable(
                name: "MailingList",
                newName: "MailingLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MailingLists",
                table: "MailingLists",
                column: "MailingList_ID");
        }
    }
}
