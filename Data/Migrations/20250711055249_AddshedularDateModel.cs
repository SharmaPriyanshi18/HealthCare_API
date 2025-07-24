using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class AddshedularDateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "schedulers",
                columns: table => new
                {
                    schedulerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    DiseaseCaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedulers", x => x.schedulerId);
                    table.ForeignKey(
                        name: "FK_schedulers_Diseases_DiseaseCaseId",
                        column: x => x.DiseaseCaseId,
                        principalTable: "Diseases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedulers_DiseaseCaseId",
                table: "schedulers",
                column: "DiseaseCaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "schedulers");
        }
    }
}
