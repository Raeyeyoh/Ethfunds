using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETFunds.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Campaigns",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Campaigns",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Campaigns",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Campaigns",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Campaigns");
        }
    }
}
