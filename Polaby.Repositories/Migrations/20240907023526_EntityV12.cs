using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "SafeFood",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "SafeFood");
        }
    }
}
