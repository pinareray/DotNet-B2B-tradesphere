using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNet_B2B_tradesphere.Migrations
{
    /// <inheritdoc />
    public partial class SeedDealers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CompanyName", "CreatedDate", "DiscountRate", "IsActive", "TaxNumber" },
                values: new object[,]
                {
                    { 1, "ABC Teknoloji Ltd.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15.0m, true, "1234567890" },
                    { 2, "XYZ Ticaret A.Ş.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10.0m, true, "9876543210" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
