using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmLock.API.Migrations
{
    /// <inheritdoc />
    public partial class AddActionTypeToDeviceActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "DeviceActionLogs",
                newName: "ActionType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "DeviceActionLogs",
                newName: "Action");
        }
    }
}
