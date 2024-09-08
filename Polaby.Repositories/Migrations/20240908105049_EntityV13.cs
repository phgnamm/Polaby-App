using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngredientSearch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Animal = table.Column<bool>(type: "bit", nullable: false),
                    Kcal = table.Column<float>(type: "real", nullable: true),
                    DisposalRate = table.Column<float>(type: "real", nullable: true),
                    FoodGroupId = table.Column<int>(type: "int", nullable: true),
                    IndexFoodGroup = table.Column<int>(type: "int", nullable: true),
                    FoodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Protein = table.Column<float>(type: "real", nullable: true),
                    Carbohydrates = table.Column<float>(type: "real", nullable: true),
                    Fat = table.Column<float>(type: "real", nullable: true),
                    Water = table.Column<float>(type: "real", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_IngredientSearch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngredientSearchNutrient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientSearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NutrientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_IngredientSearchNutrient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientSearchNutrient_IngredientSearch_IngredientSearchId",
                        column: x => x.IngredientSearchId,
                        principalTable: "IngredientSearch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IngredientSearchNutrient_Nutrient_NutrientId",
                        column: x => x.NutrientId,
                        principalTable: "Nutrient",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientSearchNutrient_IngredientSearchId",
                table: "IngredientSearchNutrient",
                column: "IngredientSearchId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientSearchNutrient_NutrientId",
                table: "IngredientSearchNutrient",
                column: "NutrientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientSearchNutrient");

            migrationBuilder.DropTable(
                name: "IngredientSearch");
        }
    }
}
