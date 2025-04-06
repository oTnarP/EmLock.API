using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmLock.API.Migrations
{
    /// <inheritdoc />
    public partial class AddShopkeeperLicenseFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseEndDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseStartDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseEndDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LicenseStartDate",
                table: "Users");
        }
    }
}
