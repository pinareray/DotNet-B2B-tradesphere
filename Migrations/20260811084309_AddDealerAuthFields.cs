using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet_B2B_tradesphere.Migrations
{
    /// <inheritdoc />
    public partial class AddDealerAuthFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Dealers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Dealers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Dealers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash", "Role" },
                values: new object[] { "abc@tradesphere.com", "", "Dealer" });

            migrationBuilder.UpdateData(
                table: "Dealers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "PasswordHash", "Role" },
                values: new object[] { "xyz@tradesphere.com", "", "Dealer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Dealers");
        }
    }
}
