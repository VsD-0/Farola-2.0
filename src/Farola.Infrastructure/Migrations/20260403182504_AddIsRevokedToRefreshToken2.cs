using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farola.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRevokedToRefreshToken2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "refresh_tokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "refresh_tokens");
        }
    }
}
