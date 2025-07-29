using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_schedulers_schedulerDateschedulerId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_schedulerDateschedulerId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "schedulerDateschedulerId",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_schedulerId",
                table: "Assessments",
                column: "schedulerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_schedulers_schedulerId",
                table: "Assessments",
                column: "schedulerId",
                principalTable: "schedulers",
                principalColumn: "schedulerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_schedulers_schedulerId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_schedulerId",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "schedulerDateschedulerId",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_schedulerDateschedulerId",
                table: "Assessments",
                column: "schedulerDateschedulerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_schedulers_schedulerDateschedulerId",
                table: "Assessments",
                column: "schedulerDateschedulerId",
                principalTable: "schedulers",
                principalColumn: "schedulerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
