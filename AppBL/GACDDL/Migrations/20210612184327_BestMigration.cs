using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GACDDL.Migrations
{
    public partial class BestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfParticipants",
                table: "Competitions",
                newName: "UserCreatedId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Competitions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionName",
                table: "Competitions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Competitions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Competitions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TestString",
                table: "Competitions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Competitions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_CategoryId",
                table: "Competitions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_UserId",
                table: "Competitions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Categories_CategoryId",
                table: "Competitions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Users_UserId",
                table: "Competitions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Categories_CategoryId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Users_UserId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_CategoryId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_UserId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "CompetitionName",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "TestString",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Competitions");

            migrationBuilder.RenameColumn(
                name: "UserCreatedId",
                table: "Competitions",
                newName: "NumberOfParticipants");
        }
    }
}
