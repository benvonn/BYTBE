using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CornHoleRevamp.Migrations
{
    /// <inheritdoc />
    public partial class StatsRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Player2TotalAfter",
                table: "RoundData",
                newName: "Player2TotalBefore");

            migrationBuilder.RenameColumn(
                name: "Player1TotalAfter",
                table: "RoundData",
                newName: "Player1TotalBefore");

            migrationBuilder.AddColumn<int>(
                name: "GameMatchId1",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player1RoundBagsIn",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player1RoundBagsOn",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player2RoundBagsIn",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player2RoundBagsOn",
                table: "RoundData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "RoundData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "WinnerName",
                table: "GameMatches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Player1RoundBagsIn",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "Player1RoundBagsOn",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "Player2RoundBagsIn",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "Player2RoundBagsOn",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "RoundData");

            migrationBuilder.DropColumn(
                name: "WinnerName",
                table: "GameMatches");

            migrationBuilder.RenameColumn(
                name: "Player2TotalBefore",
                table: "RoundData",
                newName: "Player2TotalAfter");

            migrationBuilder.RenameColumn(
                name: "Player1TotalBefore",
                table: "RoundData",
                newName: "Player1TotalAfter");
        }
    }
}
