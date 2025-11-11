using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Assets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Floor = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Room = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    BuildingId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("33333033-3333-3333-3333-233333333333"), "Block-B" },
                    { new Guid("33333333-3783-3333-3333-333333333333"), "Block-A" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LocationId",
                table: "Assets",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Building_Name",
                table: "Building",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_BuildingId",
                table: "Location",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Location_LocationId",
                table: "Assets",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Location_LocationId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropIndex(
                name: "IX_Assets_LocationId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Assets");
        }
    }
}
