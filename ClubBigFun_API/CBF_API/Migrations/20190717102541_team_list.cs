using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class team_list : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pool_Master",
                columns: table => new
                {
                    Pool_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pool_Name = table.Column<string>(maxLength: 250, nullable: true),
                    Sport_Type = table.Column<int>(nullable: false),
                    Rules_URL = table.Column<string>(maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Is_Active = table.Column<bool>(nullable: false),
                    Cut_Off = table.Column<DateTime>(nullable: false),
                    Is_Started = table.Column<bool>(nullable: false),
                    Is_Deleted = table.Column<bool>(nullable: false),
                    Last_Edited_By = table.Column<int>(nullable: false),
                    DTS = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pool_Master", x => x.Pool_ID);
                });

            migrationBuilder.CreateTable(
                name: "Sports_Type",
                columns: table => new
                {
                    SportType = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SportTypeName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports_Type", x => x.SportType);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pool_Master");

            migrationBuilder.DropTable(
                name: "Sports_Type");
        }
    }
}
