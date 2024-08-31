using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "NoteEmotion",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "NoteEmotion");
        }
    }
}
