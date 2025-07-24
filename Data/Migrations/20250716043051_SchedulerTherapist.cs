using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class SchedulerTherapist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "schedulerTherapists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulerId = table.Column<int>(type: "int", nullable: false),
                    TherapistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedulerTherapists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_schedulerTherapists_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "TherapistId");
                    table.ForeignKey(
                        name: "FK_schedulerTherapists_schedulers_SchedulerId",
                        column: x => x.SchedulerId,
                        principalTable: "schedulers",
                        principalColumn: "schedulerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedulerTherapists_SchedulerId",
                table: "schedulerTherapists",
                column: "SchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_schedulerTherapists_TherapistId",
                table: "schedulerTherapists",
                column: "TherapistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "schedulerTherapists");
        }
    }
}
