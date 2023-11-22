using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Product.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10f2d5ae-1603-4198-931b-53d075006aee"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e6356370-fcc4-47a4-8116-f62ffd566938"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("20ea3b5f-f164-49fb-94cc-ee1dcd56d92f"), "1", "Admin", "Admin" },
                    { new Guid("dbd2133b-0819-4512-ba93-1fd6e7f725b1"), "2", "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "Email", "EmailConfirmed", "FirstName", "LastName", "LastUpdateDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d3970030-e120-435e-af3c-9e2214256b7e"), 0, "ab8d4eb8-627a-4ed8-b33e-26b300720bc3", new DateTime(2023, 11, 22, 13, 16, 2, 50, DateTimeKind.Local).AddTicks(107), "admin@example.com", true, "Admin", "Admin", new DateTime(2023, 11, 22, 13, 16, 2, 50, DateTimeKind.Local).AddTicks(96), false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEDsbSXhbvdyu6NTAj4+IBHgifUqh2eTrzli8N/g9/UymFBajNzhg29wQNQN72oENQA==", null, false, "", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("20ea3b5f-f164-49fb-94cc-ee1dcd56d92f"), new Guid("d3970030-e120-435e-af3c-9e2214256b7e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dbd2133b-0819-4512-ba93-1fd6e7f725b1"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("20ea3b5f-f164-49fb-94cc-ee1dcd56d92f"), new Guid("d3970030-e120-435e-af3c-9e2214256b7e") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20ea3b5f-f164-49fb-94cc-ee1dcd56d92f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d3970030-e120-435e-af3c-9e2214256b7e"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10f2d5ae-1603-4198-931b-53d075006aee"), "2", "User", "User" },
                    { new Guid("e6356370-fcc4-47a4-8116-f62ffd566938"), "1", "Admin", "Admin" }
                });
        }
    }
}
