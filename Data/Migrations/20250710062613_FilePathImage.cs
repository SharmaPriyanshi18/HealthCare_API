using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare_API.Migrations
{
    /// <inheritdoc />
    public partial class FilePathImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ImageUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ImageUsers");
        }
    }
}
