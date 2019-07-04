using Microsoft.EntityFrameworkCore.Migrations;

namespace SustainableChemistryWeb.Migrations.SustainableChemistry
{
    public partial class AddFunctionalGroupUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "app_functionalgroup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "app_functionalgroup");
        }
    }
}
