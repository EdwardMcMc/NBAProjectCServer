using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NbaDb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    W_PCT = table.Column<double>(nullable: false),
                    MIN = table.Column<double>(nullable: false),
                    FGA = table.Column<double>(nullable: false),
                    FG3A = table.Column<double>(nullable: false),
                    FTA = table.Column<double>(nullable: false),
                    OREB = table.Column<double>(nullable: false),
                    DREB = table.Column<double>(nullable: false),
                    AST = table.Column<double>(nullable: false),
                    TOV = table.Column<double>(nullable: false),
                    STL = table.Column<double>(nullable: false),
                    BLK = table.Column<double>(nullable: false),
                    PF = table.Column<double>(nullable: false),
                    PTS = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamesPlayed",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    GP = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesPlayed", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_GamesPlayed_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayer",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => new { x.PlayerId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayer_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Team",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayer_TeamId",
                table: "TeamPlayer",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesPlayed");

            migrationBuilder.DropTable(
                name: "TeamPlayer");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
