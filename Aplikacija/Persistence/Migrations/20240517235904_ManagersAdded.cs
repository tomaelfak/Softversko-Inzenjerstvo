using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ManagersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Courts",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Courts_ManagerId",
                table: "Courts",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courts_AspNetUsers_ManagerId",
                table: "Courts",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courts_AspNetUsers_ManagerId",
                table: "Courts");

            migrationBuilder.DropIndex(
                name: "IX_Courts_ManagerId",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Courts");
        }
    }
}
