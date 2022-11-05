using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class MultiplePrefferedMachines2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_People_PersonId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_PersonId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Machines");

            migrationBuilder.CreateTable(
                name: "MachinePerson",
                columns: table => new
                {
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    PreferredMachinesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachinePerson", x => new { x.PeopleId, x.PreferredMachinesId });
                    table.ForeignKey(
                        name: "FK_MachinePerson_Machines_PreferredMachinesId",
                        column: x => x.PreferredMachinesId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachinePerson_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachinePerson_PreferredMachinesId",
                table: "MachinePerson",
                column: "PreferredMachinesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachinePerson");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Machines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_PersonId",
                table: "Machines",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_People_PersonId",
                table: "Machines",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
