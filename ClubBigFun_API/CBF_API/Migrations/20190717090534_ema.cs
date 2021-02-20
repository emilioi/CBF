using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class ema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Email_Templates",
                columns: table => new
                {
                    Template_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Template_Subject = table.Column<string>(maxLength: 250, nullable: true),
                    Template_Description = table.Column<string>(nullable: true),
                    Template_For = table.Column<string>(maxLength: 250, nullable: true),
                    Is_Deleted = table.Column<byte>(nullable: false),
                    Last_Edited_By = table.Column<int>(nullable: false),
                    DTS = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Templates", x => x.Template_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email_Templates");
        }
    }
}
