using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carsure.Migrations
{
    /// <inheritdoc />
    public partial class LinkAdToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing ads that have no CarId (old seed data)
            migrationBuilder.Sql("DELETE FROM [Ads]");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_CarId",
                table: "Ads",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Cars_CarId",
                table: "Ads",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Cars_CarId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_CarId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Ads");
        }
    }
}
