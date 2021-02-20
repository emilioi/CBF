using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class emailNotificationn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Email_Notification",
                columns: table => new
                {
                    Notification = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    To_Email = table.Column<string>(maxLength: 100, nullable: true),
                    Subject = table.Column<string>(maxLength: 500, nullable: true),
                    Member_Id = table.Column<int>(nullable: false),
                    Email_Content = table.Column<string>(nullable: true),
                    From_Email = table.Column<string>(maxLength: 100, nullable: true),
                    Is_Sent = table.Column<bool>(nullable: false),
                    Failed_Error = table.Column<DateTime>(maxLength: 500, nullable: false),
                    Sent_On = table.Column<string>(nullable: true),
                    DTS = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Notification", x => x.Notification);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email_Notification");
        }
    }
}
