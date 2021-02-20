using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class Production : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorExceptions",
                columns: table => new
                {
                    Log_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    date = table.Column<DateTime>(nullable: false),
                    Thread = table.Column<string>(nullable: true),
                    Context = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    User_Id = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorExceptions", x => x.Log_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorExceptions");
        }
    }
}
