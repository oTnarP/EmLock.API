using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmLock.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletFreezeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNote",
                table: "Wallets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFrozen",
                table: "Wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNote",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "IsFrozen",
                table: "Wallets");
        }
    }
}
