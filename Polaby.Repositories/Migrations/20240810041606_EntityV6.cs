using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Starch",
                table: "Dish",
                newName: "Weight");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "Nutrient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Carbohydrates",
                table: "Dish",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrient_DishId",
                table: "Nutrient",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrient_Dish_DishId",
                table: "Nutrient",
                column: "DishId",
                principalTable: "Dish",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nutrient_Dish_DishId",
                table: "Nutrient");

            migrationBuilder.DropIndex(
                name: "IX_Nutrient_DishId",
                table: "Nutrient");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Nutrient");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Dish");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Dish",
                newName: "Starch");
        }
    }
}
