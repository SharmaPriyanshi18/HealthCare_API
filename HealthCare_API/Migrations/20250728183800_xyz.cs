using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class xyz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedulers_Diseases_DiseaseCaseId",
                table: "schedulers");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "schedulers");

            migrationBuilder.RenameColumn(
                name: "DiseaseCaseId",
                table: "schedulers",
                newName: "treatmentId");

            migrationBuilder.RenameIndex(
                name: "IX_schedulers_DiseaseCaseId",
                table: "schedulers",
                newName: "IX_schedulers_treatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_schedulers_treatments_treatmentId",
                table: "schedulers",
                column: "treatmentId",
                principalTable: "treatments",
                principalColumn: "treatmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedulers_treatments_treatmentId",
                table: "schedulers");

            migrationBuilder.RenameColumn(
                name: "treatmentId",
                table: "schedulers",
                newName: "DiseaseCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_schedulers_treatmentId",
                table: "schedulers",
                newName: "IX_schedulers_DiseaseCaseId");

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "schedulers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_schedulers_Diseases_DiseaseCaseId",
                table: "schedulers",
                column: "DiseaseCaseId",
                principalTable: "Diseases",
                principalColumn: "CaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
