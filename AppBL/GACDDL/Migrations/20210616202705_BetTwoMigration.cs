using Microsoft.EntityFrameworkCore.Migrations;

namespace GACDDL.Migrations
{
    public partial class BetTwoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_CompetitionStats_CompetitionStatUserId_CompetitionStat~",
                table: "Bets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_CompetitionStatUserId_CompetitionStatCompetitionId",
                table: "Bets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_UserId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "CompetitionStatCompetitionId",
                table: "Bets");

            migrationBuilder.RenameColumn(
                name: "CompetitionStatUserId",
                table: "Bets",
                newName: "CompetitionId");

            migrationBuilder.RenameColumn(
                name: "CompetitionStatId",
                table: "Bets",
                newName: "BettingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId_CompetitionId",
                table: "Bets",
                columns: new[] { "UserId", "CompetitionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_CompetitionStats_UserId_CompetitionId",
                table: "Bets",
                columns: new[] { "UserId", "CompetitionId" },
                principalTable: "CompetitionStats",
                principalColumns: new[] { "UserId", "CompetitionId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_CompetitionStats_UserId_CompetitionId",
                table: "Bets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_UserId_CompetitionId",
                table: "Bets");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "Bets",
                newName: "CompetitionStatUserId");

            migrationBuilder.RenameColumn(
                name: "BettingUserId",
                table: "Bets",
                newName: "CompetitionStatId");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionStatCompetitionId",
                table: "Bets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bets_CompetitionStatUserId_CompetitionStatCompetitionId",
                table: "Bets",
                columns: new[] { "CompetitionStatUserId", "CompetitionStatCompetitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId",
                table: "Bets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_CompetitionStats_CompetitionStatUserId_CompetitionStat~",
                table: "Bets",
                columns: new[] { "CompetitionStatUserId", "CompetitionStatCompetitionId" },
                principalTable: "CompetitionStats",
                principalColumns: new[] { "UserId", "CompetitionId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
