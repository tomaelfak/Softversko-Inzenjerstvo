using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PrivateActivityBezNotMapped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Teams_CourtId",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TeamId",
                table: "Activities",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Teams_TeamId",
                table: "Activities",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Teams_TeamId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_TeamId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Teams_CourtId",
                table: "Activities",
                column: "CourtId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
