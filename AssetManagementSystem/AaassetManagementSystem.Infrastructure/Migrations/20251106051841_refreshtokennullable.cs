using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refreshtokennullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("0b9c7db2-93d8-4ddd-9842-89bfa0cf2d76"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("1881f2a5-39e5-4db3-88bb-9abecc88a237"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("3ec1410a-d735-4516-a3b5-235b905e8b58"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("8b871c99-f8d3-4326-bff5-15cade7e1a6a"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("ea12d78f-f555-4d26-b289-5da3bc24ec29"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0c9e30eb-2775-46b8-8fb3-f14a6e808586"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2df42591-a865-40ee-817d-c2eaf6891ca2"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefereshTokenExpiry",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RefereshToken",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("0e4ef4ea-a36a-460f-b14d-78467f86e9eb"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(6729), "System", "Manages employee relations and administrative equipment.", true, null, null, "Human Resources" },
                    { new Guid("4ee9e790-cfe0-4134-981b-908206fe9fb4"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(6738), "System", "Responsible for accounting systems, finance tools, and related devices.", true, null, null, "Finance" },
                    { new Guid("9f0346ce-97be-4009-8e6c-ffe6d48b5408"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(6740), "System", "Oversees logistics, facilities, and maintenance assets.", true, null, null, "Operations" },
                    { new Guid("a21c533c-6abf-49ab-bfd9-7dbcd569e0ef"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(6724), "System", "Handles software, hardware, and infrastructure-related assets.", true, null, null, "Information Technology" },
                    { new Guid("c09a5e40-9422-486a-ae83-096e1bccc753"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(6743), "System", "Manages branding equipment, digital tools, and promotional materials.", true, null, null, "Marketing" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "RoleName" },
                values: new object[,]
                {
                    { new Guid("9bbac780-3662-4367-a1bb-15f4a3ef0db4"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(4501), "System", null, true, null, null, "User" },
                    { new Guid("a19a80bc-ee82-475d-ac3b-6360c94ea3c2"), new DateTime(2025, 11, 6, 5, 18, 41, 138, DateTimeKind.Utc).AddTicks(4497), "System", null, true, null, null, "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("0e4ef4ea-a36a-460f-b14d-78467f86e9eb"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("4ee9e790-cfe0-4134-981b-908206fe9fb4"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("9f0346ce-97be-4009-8e6c-ffe6d48b5408"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("a21c533c-6abf-49ab-bfd9-7dbcd569e0ef"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("c09a5e40-9422-486a-ae83-096e1bccc753"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9bbac780-3662-4367-a1bb-15f4a3ef0db4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a19a80bc-ee82-475d-ac3b-6360c94ea3c2"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefereshTokenExpiry",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefereshToken",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("0b9c7db2-93d8-4ddd-9842-89bfa0cf2d76"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(5354), "System", "Responsible for accounting systems, finance tools, and related devices.", true, null, null, "Finance" },
                    { new Guid("1881f2a5-39e5-4db3-88bb-9abecc88a237"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(5346), "System", "Handles software, hardware, and infrastructure-related assets.", true, null, null, "Information Technology" },
                    { new Guid("3ec1410a-d735-4516-a3b5-235b905e8b58"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(5351), "System", "Manages employee relations and administrative equipment.", true, null, null, "Human Resources" },
                    { new Guid("8b871c99-f8d3-4326-bff5-15cade7e1a6a"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(5358), "System", "Oversees logistics, facilities, and maintenance assets.", true, null, null, "Operations" },
                    { new Guid("ea12d78f-f555-4d26-b289-5da3bc24ec29"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(5364), "System", "Manages branding equipment, digital tools, and promotional materials.", true, null, null, "Marketing" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "RoleName" },
                values: new object[,]
                {
                    { new Guid("0c9e30eb-2775-46b8-8fb3-f14a6e808586"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(2970), "System", null, true, null, null, "User" },
                    { new Guid("2df42591-a865-40ee-817d-c2eaf6891ca2"), new DateTime(2025, 11, 5, 10, 5, 40, 617, DateTimeKind.Utc).AddTicks(2965), "System", null, true, null, null, "Admin" }
                });
        }
    }
}
