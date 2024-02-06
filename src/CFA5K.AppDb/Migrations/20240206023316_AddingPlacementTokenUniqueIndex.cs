using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFA5K.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class AddingPlacementTokenUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlacementTokens_OccasionId",
                table: "PlacementTokens");

            migrationBuilder.CreateIndex(
                name: "IX_PlacementTokens_OccasionId_Position",
                table: "PlacementTokens",
                columns: new[] { "OccasionId", "Position" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlacementTokens_OccasionId_Position",
                table: "PlacementTokens");

            migrationBuilder.CreateIndex(
                name: "IX_PlacementTokens_OccasionId",
                table: "PlacementTokens",
                column: "OccasionId");
        }
    }
}
