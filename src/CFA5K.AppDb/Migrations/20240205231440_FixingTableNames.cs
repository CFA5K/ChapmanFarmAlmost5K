using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CFA5K.AppDb.Migrations
{
    /// <inheritdoc />
    public partial class FixingTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlacementToken_Occasion_OccasionId",
                table: "PlacementToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlacementToken",
                table: "PlacementToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occasion",
                table: "Occasion");

            migrationBuilder.RenameTable(
                name: "PlacementToken",
                newName: "PlacementTokens");

            migrationBuilder.RenameTable(
                name: "Occasion",
                newName: "Occasions");

            migrationBuilder.RenameIndex(
                name: "IX_PlacementToken_OccasionId",
                table: "PlacementTokens",
                newName: "IX_PlacementTokens_OccasionId");

            migrationBuilder.RenameIndex(
                name: "IX_Occasion_Name",
                table: "Occasions",
                newName: "IX_Occasions_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlacementTokens",
                table: "PlacementTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occasions",
                table: "Occasions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementTokens_Occasions_OccasionId",
                table: "PlacementTokens",
                column: "OccasionId",
                principalTable: "Occasions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlacementTokens_Occasions_OccasionId",
                table: "PlacementTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlacementTokens",
                table: "PlacementTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occasions",
                table: "Occasions");

            migrationBuilder.RenameTable(
                name: "PlacementTokens",
                newName: "PlacementToken");

            migrationBuilder.RenameTable(
                name: "Occasions",
                newName: "Occasion");

            migrationBuilder.RenameIndex(
                name: "IX_PlacementTokens_OccasionId",
                table: "PlacementToken",
                newName: "IX_PlacementToken_OccasionId");

            migrationBuilder.RenameIndex(
                name: "IX_Occasions_Name",
                table: "Occasion",
                newName: "IX_Occasion_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlacementToken",
                table: "PlacementToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occasion",
                table: "Occasion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementToken_Occasion_OccasionId",
                table: "PlacementToken",
                column: "OccasionId",
                principalTable: "Occasion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
