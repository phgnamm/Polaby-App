using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calcium",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Cholesterol",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Fiber",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Iron",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Magnesium",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "MonounsaturatedFat",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "PolyunsaturatedFat",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Potassium",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "SaturatedFat",
                table: "Ingredient");

            migrationBuilder.RenameColumn(
                name: "Zinc",
                table: "Ingredient",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Water",
                table: "Ingredient",
                newName: "NumberOfDecimalPart");

            migrationBuilder.RenameColumn(
                name: "Sugar",
                table: "Ingredient",
                newName: "KcalDefault");

            migrationBuilder.RenameColumn(
                name: "Starch",
                table: "Ingredient",
                newName: "DisposalRate");

            migrationBuilder.RenameColumn(
                name: "Sodium",
                table: "Ingredient",
                newName: "Alco");

            migrationBuilder.AddColumn<bool>(
                name: "Animal",
                table: "Ingredient",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FoodGroup",
                table: "Ingredient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodGroupId",
                table: "Ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndexFoodGroup",
                table: "Ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nutrient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostProcessingAmount = table.Column<float>(type: "real", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionId = table.Column<int>(type: "int", nullable: true),
                    ConversionRate = table.Column<float>(type: "real", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FractionalQuantity = table.Column<int>(type: "int", nullable: true),
                    NutritionCode = table.Column<int>(type: "int", nullable: true),
                    IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nutrient_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nutrient_IngredientId",
                table: "Nutrient",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nutrient");

            migrationBuilder.DropColumn(
                name: "Animal",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "FoodGroup",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "FoodGroupId",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "IndexFoodGroup",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredient");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Ingredient",
                newName: "Zinc");

            migrationBuilder.RenameColumn(
                name: "NumberOfDecimalPart",
                table: "Ingredient",
                newName: "Water");

            migrationBuilder.RenameColumn(
                name: "KcalDefault",
                table: "Ingredient",
                newName: "Sugar");

            migrationBuilder.RenameColumn(
                name: "DisposalRate",
                table: "Ingredient",
                newName: "Starch");

            migrationBuilder.RenameColumn(
                name: "Alco",
                table: "Ingredient",
                newName: "Sodium");

            migrationBuilder.AddColumn<float>(
                name: "Calcium",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cholesterol",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Fiber",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Iron",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Magnesium",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MonounsaturatedFat",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PolyunsaturatedFat",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Potassium",
                table: "Ingredient",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SaturatedFat",
                table: "Ingredient",
                type: "real",
                nullable: true);
        }
    }
}
