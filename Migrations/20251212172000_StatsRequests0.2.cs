using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CornHoleRevamp.Migrations
{
    /// <inheritdoc />
    public partial class StatsRequests02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundData_GameMatches_GameMatchId1",
                table: "RoundData");

            migrationBuilder.DropIndex(
                name: "IX_RoundData_GameMatchId1",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "GameMatchId1",
                table: "RoundData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameMatchId1",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoundData_GameMatchId1",
                table: "RoundData",
                column: "GameMatchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundData_GameMatches_GameMatchId1",
                table: "RoundData",
                column: "GameMatchId1",
                principalTable: "GameMatches",
                principalColumn: "Id");
        }
    }
}
