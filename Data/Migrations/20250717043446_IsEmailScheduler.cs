using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class IsEmailScheduler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailSent",
                table: "schedulers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailSent",
                table: "schedulers");
        }
    }
}
