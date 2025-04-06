using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmLock.API.Migrations
{
    /// <inheritdoc />
    public partial class Add2FAFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is2FAEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKey",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is2FAEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "Users");
        }
    }
}
