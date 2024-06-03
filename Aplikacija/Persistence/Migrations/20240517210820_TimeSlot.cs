using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId",
                table: "Activities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTime = table.Column<int>(type: "INTEGER", nullable: false),
                    EndTime = table.Column<int>(type: "INTEGER", nullable: false),
                    CourtId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlot_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TimeSlotId",
                table: "Activities",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_CourtId",
                table: "TimeSlot",
                column: "CourtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_TimeSlot_TimeSlotId",
                table: "Activities",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_TimeSlot_TimeSlotId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_Activities_TimeSlotId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Activities");
        }
    }
}
