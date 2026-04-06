using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farola.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceFingerprintToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceFingerprint",
                table: "refresh_tokens",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceFingerprint",
                table: "refresh_tokens");
        }
    }
}
