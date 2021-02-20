using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBF_API.Migrations
{
    public partial class pools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rules_URL",
                table: "Pool_Master",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

           
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
             

            migrationBuilder.AlterColumn<string>(
                name: "Rules_URL",
                table: "Pool_Master",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
