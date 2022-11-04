using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class MachineQualificationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Qualifications_Id",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Machines",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "QualificationId",
                table: "Machines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_QualificationId",
                table: "Machines",
                column: "QualificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Qualifications_QualificationId",
                table: "Machines",
                column: "QualificationId",
                principalTable: "Qualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Qualifications_QualificationId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_QualificationId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "QualificationId",
                table: "Machines");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Machines",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Qualifications_Id",
                table: "Machines",
                column: "Id",
                principalTable: "Qualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
