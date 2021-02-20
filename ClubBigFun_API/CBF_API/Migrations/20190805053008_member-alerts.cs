using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class memberalerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Member_Alerts",
                columns: table => new
                {
                    Alert_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alert_Name = table.Column<string>(nullable: true),
                    Last_Reminder = table.Column<DateTime>(nullable: false),
                    Next_Reminder = table.Column<DateTime>(nullable: false),
                    Is_AfterLogin = table.Column<bool>(nullable: false),
                    One_TimeReminder = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member_Alerts", x => x.Alert_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Member_Alerts");
        }
    }
}
