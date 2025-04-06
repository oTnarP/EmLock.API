using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmLock.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLicenseFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLicenseActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLicenseActive",
                table: "Users");
        }
    }
}
