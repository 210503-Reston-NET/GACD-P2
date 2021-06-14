using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GACDDL.Migrations
{
    public partial class CompChangeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserStatCatJoins");

            migrationBuilder.AddColumn<string>(
                name: "TestAuthor",
                table: "Competitions",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins",
                columns: new[] { "UserStatId", "UserId", "CategoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStatCatJoins",
                table: "UserStatCatJoins");

            migrationBuilder.DropColumn(
                name: "TestAuthor",
                table: "Competitions");

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
        }
    }
}
