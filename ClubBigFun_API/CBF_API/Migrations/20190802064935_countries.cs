using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class countries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Countries",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(maxLength: 150, nullable: true),
            //        Short_Name = table.Column<string>(maxLength: 3, nullable: true),
            //        PhoneCode = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Countries", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "States",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(maxLength: 30, nullable: true),
            //        Country_ID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_States", x => x.ID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
