using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farola.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceInfoToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "refresh_tokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "refresh_tokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "refresh_tokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "refresh_tokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "refresh_tokens");
        }
    }
}
