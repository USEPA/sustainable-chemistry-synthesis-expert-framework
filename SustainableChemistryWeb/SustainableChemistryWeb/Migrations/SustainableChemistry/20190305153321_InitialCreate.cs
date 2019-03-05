using Microsoft.EntityFrameworkCore.Migrations;

namespace SustainableChemistryWeb.Migrations.SustainableChemistry
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_catalyst",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_catalyst", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_compound",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: false),
                    CasNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_compound", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_functionalgroup",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    Smarts = table.Column<string>(type: "varchar(150)", nullable: false),
                    Image = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_functionalgroup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_reactant",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Temp2 = table.Column<string>(type: "varchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_reactant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_solvent",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_solvent", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_namedreaction",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    ReactantA = table.Column<string>(type: "varchar(150)", nullable: false),
                    ReactantB = table.Column<string>(type: "varchar(150)", nullable: false),
                    ReactantC = table.Column<string>(type: "varchar(150)", nullable: false),
                    Product = table.Column<string>(type: "varchar(150)", nullable: false),
                    Heat = table.Column<string>(type: "varchar(2)", nullable: false),
                    AcidBase = table.Column<string>(type: "varchar(2)", nullable: false),
                    Image = table.Column<string>(type: "varchar(100)", nullable: false),
                    Catalyst_id = table.Column<long>(nullable: false),
                    Functional_Group_id = table.Column<long>(nullable: false),
                    Solvent_id = table.Column<long>(nullable: false),
                    URL = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_namedreaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_app_catalyst_Catalyst_id",
                        column: x => x.Catalyst_id,
                        principalTable: "app_catalyst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_app_functionalgroup_Functional_Group_id",
                        column: x => x.Functional_Group_id,
                        principalTable: "app_functionalgroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_app_solvent_Solvent_id",
                        column: x => x.Solvent_id,
                        principalTable: "app_solvent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_namedreaction_ByProducts",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    namedreaction_id = table.Column<long>(nullable: false),
                    reactant_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_namedreaction_ByProducts", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_ByProducts_app_namedreaction_namedreaction_id",
                        column: x => x.namedreaction_id,
                        principalTable: "app_namedreaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_ByProducts_app_reactant_reactant_id",
                        column: x => x.reactant_id,
                        principalTable: "app_reactant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_namedreaction_Reactants",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    namedreaction_id = table.Column<long>(nullable: false),
                    reactant_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_namedreaction_Reactants", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_Reactants_app_namedreaction_namedreaction_id",
                        column: x => x.namedreaction_id,
                        principalTable: "app_namedreaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_namedreaction_Reactants_app_reactant_reactant_id",
                        column: x => x.reactant_id,
                        principalTable: "app_reactant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_reference",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RISData = table.Column<string>(nullable: false),
                    Functional_Group_id = table.Column<long>(nullable: true),
                    Reaction_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_reference", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_reference_app_functionalgroup_Functional_Group_id",
                        column: x => x.Functional_Group_id,
                        principalTable: "app_functionalgroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_reference_app_namedreaction_Reaction_id",
                        column: x => x.Reaction_id,
                        principalTable: "app_namedreaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_catalyst_Name",
                table: "app_catalyst",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_compound_Name",
                table: "app_compound",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_functionalgroup_Name",
                table: "app_functionalgroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_functionalgroup_Smarts",
                table: "app_functionalgroup",
                column: "Smarts",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Catalyst_id_63600e1e",
                table: "app_namedreaction",
                column: "Catalyst_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Functional_Group_id_057af1bd",
                table: "app_namedreaction",
                column: "Functional_Group_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Solvent_id_7ec52782",
                table: "app_namedreaction",
                column: "Solvent_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Functional_Group_id_Name_5a5a6724_uniq",
                table: "app_namedreaction",
                columns: new[] { "Functional_Group_id", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_ByProducts_namedreaction_id_a2dc2fd2",
                table: "app_namedreaction_ByProducts",
                column: "namedreaction_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_ByProducts_reactant_id_fc608f72",
                table: "app_namedreaction_ByProducts",
                column: "reactant_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_ByProducts_namedreaction_id_reactant_id_0784f477_uniq",
                table: "app_namedreaction_ByProducts",
                columns: new[] { "namedreaction_id", "reactant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Reactants_namedreaction_id_b07b57ce",
                table: "app_namedreaction_Reactants",
                column: "namedreaction_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Reactants_reactant_id_5118ff6c",
                table: "app_namedreaction_Reactants",
                column: "reactant_id");

            migrationBuilder.CreateIndex(
                name: "app_namedreaction_Reactants_namedreaction_id_reactant_id_a9ecf412_uniq",
                table: "app_namedreaction_Reactants",
                columns: new[] { "namedreaction_id", "reactant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_reactant_Name",
                table: "app_reactant",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "app_reference_Functional_Group_id_b8927bac",
                table: "app_reference",
                column: "Functional_Group_id");

            migrationBuilder.CreateIndex(
                name: "app_reference_Reaction_id_bf824395",
                table: "app_reference",
                column: "Reaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_solvent_Name",
                table: "app_solvent",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_compound");

            migrationBuilder.DropTable(
                name: "app_namedreaction_ByProducts");

            migrationBuilder.DropTable(
                name: "app_namedreaction_Reactants");

            migrationBuilder.DropTable(
                name: "app_reference");

            migrationBuilder.DropTable(
                name: "app_reactant");

            migrationBuilder.DropTable(
                name: "app_namedreaction");

            migrationBuilder.DropTable(
                name: "app_catalyst");

            migrationBuilder.DropTable(
                name: "app_functionalgroup");

            migrationBuilder.DropTable(
                name: "app_solvent");
        }
    }
}
