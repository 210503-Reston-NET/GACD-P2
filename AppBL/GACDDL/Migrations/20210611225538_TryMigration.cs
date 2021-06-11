using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GACDDL.Migrations
{
    public partial class TryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserStatCatJoins",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatCatJoins_UserId",
                table: "UserStatCatJoins",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins");

            migrationBuilder.DropIndex(
                name: "IX_UserStatCatJoins_UserId",
                table: "UserStatCatJoins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserStatCatJoins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins",
                columns: new[] { "UserId", "UserStatId", "CategoryId" });
        }
    }
}
