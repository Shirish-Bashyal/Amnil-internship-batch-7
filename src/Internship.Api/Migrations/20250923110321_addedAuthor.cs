using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1838));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1842));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1861));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1862));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1806));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 11, 3, 21, 241, DateTimeKind.Utc).AddTicks(1808));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8327));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8329));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8339));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8358));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8360));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8297));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 23, 10, 53, 14, 151, DateTimeKind.Utc).AddTicks(8299));
        }
    }
}
