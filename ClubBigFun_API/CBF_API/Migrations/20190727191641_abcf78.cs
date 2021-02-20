using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class abcf78 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_survPoolWeekList",
            //    table: "survPoolWeekList");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_qrySurvScheduleList",
            //    table: "qrySurvScheduleList");

            //migrationBuilder.DropColumn(
            //    name: "Pool_Name",
            //    table: "survPoolWeekList");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ScheduleID",
            //    table: "qrySurvScheduleList",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "PoolID",
            //    table: "qrySurvScheduleList",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Login_ID",
            //    table: "qrySurvEntries",
            //    nullable: true,
            //    oldClrType: typeof(int));

            //migrationBuilder.AlterColumn<int>(
            //    name: "Defaults",
            //    table: "qrySurvEntries",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_survPoolWeekList",
                table: "survPoolWeekList",
                columns: new[] { "PoolID", "WeekNumber" });

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_qrySurvScheduleList",
            //    table: "qrySurvScheduleList",
            //    column: "ScheduleID");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurvPoolWeekListMapped");

            migrationBuilder.DropPrimaryKey(
                name: "PK_survPoolWeekList",
                table: "survPoolWeekList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_qrySurvScheduleList",
                table: "qrySurvScheduleList");

            migrationBuilder.AddColumn<string>(
                name: "Pool_Name",
                table: "survPoolWeekList",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PoolID",
                table: "qrySurvScheduleList",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleID",
                table: "qrySurvScheduleList",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Login_ID",
                table: "qrySurvEntries",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Defaults",
                table: "qrySurvEntries",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_survPoolWeekList",
                table: "survPoolWeekList",
                column: "PoolID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_qrySurvScheduleList",
                table: "qrySurvScheduleList",
                column: "PoolID");
        }
    }
}
