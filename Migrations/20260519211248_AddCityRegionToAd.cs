using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carsure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityRegionToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Ads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Ads",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Ads");
        }
    }
}
