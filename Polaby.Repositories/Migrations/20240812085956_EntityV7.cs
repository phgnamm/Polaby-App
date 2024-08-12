using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Water",
                table: "Menu",
                newName: "Protein");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Nutrient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Alco",
                table: "Menu",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Carbohydrates",
                table: "Menu",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Fat",
                table: "Menu",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Fiber",
                table: "Menu",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrient_MenuId",
                table: "Nutrient",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrient_Menu_MenuId",
                table: "Nutrient",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nutrient_Menu_MenuId",
                table: "Nutrient");

            migrationBuilder.DropIndex(
                name: "IX_Nutrient_MenuId",
                table: "Nutrient");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Nutrient");

            migrationBuilder.DropColumn(
                name: "Alco",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Fiber",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "Menu",
                newName: "Water");
        }
    }
}
