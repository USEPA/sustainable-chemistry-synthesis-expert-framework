using Microsoft.EntityFrameworkCore.Migrations;

namespace SustainableChemistryWeb.Migrations.SustainableChemistry
{
    public partial class userStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_solvent",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_solvent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_reference",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_reference",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_reactant",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_reactant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_namedreaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_namedreaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_functionalgroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_functionalgroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_compound",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_compound",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "app_catalyst",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "app_catalyst",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_solvent");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_solvent");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_reference");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_reference");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_reactant");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_reactant");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_namedreaction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_namedreaction");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_functionalgroup");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_functionalgroup");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_compound");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_compound");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "app_catalyst");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "app_catalyst");
        }
    }
}
