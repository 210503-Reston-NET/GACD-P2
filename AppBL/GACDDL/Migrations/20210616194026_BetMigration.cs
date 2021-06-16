using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GACDDL.Migrations
{
    public partial class BetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Revapoints",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CompetitionStatId = table.Column<int>(type: "integer", nullable: false),
                    CompetitionStatUserId = table.Column<int>(type: "integer", nullable: false),
                    CompetitionStatCompetitionId = table.Column<int>(type: "integer", nullable: false),
                    PointsBet = table.Column<int>(type: "integer", nullable: false),
                    Won = table.Column<bool>(type: "boolean", nullable: false),
                    Claimed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_CompetitionStats_CompetitionStatUserId_CompetitionStat~",
                        columns: x => new { x.CompetitionStatUserId, x.CompetitionStatCompetitionId },
                        principalTable: "CompetitionStats",
                        principalColumns: new[] { "UserId", "CompetitionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_CompetitionStatUserId_CompetitionStatCompetitionId",
                table: "Bets",
                columns: new[] { "CompetitionStatUserId", "CompetitionStatCompetitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_UserId",
                table: "Bets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropColumn(
                name: "Revapoints",
                table: "Users");
        }
    }
}
