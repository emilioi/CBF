using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class weekNumber3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddUniqueConstraint(
                name: "AK_SurvEntryPicks_EntryID_ScheduleID",
                table: "SurvEntryPicks",
                columns: new[] { "EntryID", "ScheduleID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurvEntryPicks",
                table: "SurvEntryPicks",
                columns: new[] { "WeekNumber", "EntryID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
