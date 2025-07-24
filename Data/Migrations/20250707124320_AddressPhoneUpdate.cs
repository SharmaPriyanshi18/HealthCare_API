using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class AddressPhoneUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Therapists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Therapists");
        }
    }
}
