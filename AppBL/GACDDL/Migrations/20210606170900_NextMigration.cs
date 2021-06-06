using Microsoft.EntityFrameworkCore.Migrations;

namespace GACDDL.Migrations
{
    public partial class NextMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalTestTime",
                table: "UserStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeTaken",
                table: "TypeTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTestTime",
                table: "UserStats");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "TypeTests");
        }
    }
}
