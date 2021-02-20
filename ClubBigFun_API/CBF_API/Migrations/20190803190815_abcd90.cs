using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class abcd90 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Sport_Type",
            //    table: "MostPickedLists",
            //    newName: "SportType");

            //migrationBuilder.RenameColumn(
            //    name: "Short_Name",
            //    table: "Countries",
            //    newName: "SortName");

            //migrationBuilder.AddColumn<int>(
            //    name: "PickCount",
            //    table: "MostPickedLists",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "PickReport",
            //    columns: table => new
            //    {
            //        Ticket = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Pick = table.Column<string>(nullable: true),
            //        Date = table.Column<string>(nullable: true),
            //        Eliminated = table.Column<bool>(nullable: false),
            //        Defaulted = table.Column<bool>(nullable: false),
            //        Defaults = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PickReport", x => x.Ticket);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SurvEntries_WithoutPicks",
            //    columns: table => new
            //    {
            //        EntryID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        EntryName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SurvEntries_WithoutPicks", x => x.EntryID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TicketByPoolId",
            //    columns: table => new
            //    {
            //        EntryID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        EntryName = table.Column<string>(nullable: true),
            //        Name = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TicketByPoolId", x => x.EntryID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickReport");

            migrationBuilder.DropTable(
                name: "SurvEntries_WithoutPicks");

            migrationBuilder.DropTable(
                name: "TicketByPoolId");

            migrationBuilder.DropColumn(
                name: "PickCount",
                table: "MostPickedLists");

            migrationBuilder.RenameColumn(
                name: "SportType",
                table: "MostPickedLists",
                newName: "Sport_Type");

            migrationBuilder.RenameColumn(
                name: "SortName",
                table: "Countries",
                newName: "Short_Name");
        }
    }
}
