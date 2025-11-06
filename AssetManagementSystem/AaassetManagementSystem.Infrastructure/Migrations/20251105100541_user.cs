using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("499e724d-9782-4f15-ab6b-9acf7d6247ab"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("4c26af81-dc33-43c0-91c5-4ecc61c9407e"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("79ca0749-c8cf-4296-8775-36d4d3b93173"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("ca165926-a659-4e68-b36b-bd0833a35b41"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("fedd83c2-cd3a-47ff-aa2e-d4766ce07c29"));

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHashed = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    RoleID = table.Column<Guid>(type: "TEXT", nullable: true),
                    RefereshToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefereshTokenExpiry = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

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

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("499e724d-9782-4f15-ab6b-9acf7d6247ab"), new DateTime(2025, 10, 16, 12, 33, 23, 65, DateTimeKind.Utc).AddTicks(7430), "System", "Oversees logistics, facilities, and maintenance assets.", true, null, null, "Operations" },
                    { new Guid("4c26af81-dc33-43c0-91c5-4ecc61c9407e"), new DateTime(2025, 10, 16, 12, 33, 23, 65, DateTimeKind.Utc).AddTicks(7436), "System", "Manages branding equipment, digital tools, and promotional materials.", true, null, null, "Marketing" },
                    { new Guid("79ca0749-c8cf-4296-8775-36d4d3b93173"), new DateTime(2025, 10, 16, 12, 33, 23, 65, DateTimeKind.Utc).AddTicks(7426), "System", "Responsible for accounting systems, finance tools, and related devices.", true, null, null, "Finance" },
                    { new Guid("ca165926-a659-4e68-b36b-bd0833a35b41"), new DateTime(2025, 10, 16, 12, 33, 23, 65, DateTimeKind.Utc).AddTicks(7418), "System", "Handles software, hardware, and infrastructure-related assets.", true, null, null, "Information Technology" },
                    { new Guid("fedd83c2-cd3a-47ff-aa2e-d4766ce07c29"), new DateTime(2025, 10, 16, 12, 33, 23, 65, DateTimeKind.Utc).AddTicks(7423), "System", "Manages employee relations and administrative equipment.", true, null, null, "Human Resources" }
                });
        }
    }
}
