using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class weekNumber2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurvEntryPicks",
                table: "SurvEntryPicks");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_SurvEntryPicks_EntryID_ScheduleID",
                table: "SurvEntryPicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurvEntryPicks",
                table: "SurvEntryPicks");

            migrationBuilder.AlterColumn<int>(
                name: "EntryID",
                table: "SurvEntryPicks",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurvEntryPicks",
                table: "SurvEntryPicks",
                column: "EntryID");
        }
    }
}
