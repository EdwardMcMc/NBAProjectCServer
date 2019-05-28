using Microsoft.EntityFrameworkCore.Migrations;

namespace NbaDb.Migrations
{
    public partial class UpdateTeamForPredictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HashCode",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Prediction",
                table: "Team",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashCode",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Prediction",
                table: "Team");
        }
    }
}
