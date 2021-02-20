using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class mailinglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Lookup_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Lookup_Type = table.Column<string>(maxLength: 80, nullable: true),
                    Lookup_Name = table.Column<string>(maxLength: 80, nullable: true),
                    Lookup_Value = table.Column<string>(maxLength: 80, nullable: true),
                    Is_Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Lookup_Id);
                });

            migrationBuilder.CreateTable(
                name: "MailingLists",
                columns: table => new
                {
                    MailingList_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Referer = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailingLists", x => x.MailingList_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "MailingLists");
        }
    }
}
