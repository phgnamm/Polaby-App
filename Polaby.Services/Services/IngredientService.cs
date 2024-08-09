
using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.IngredientModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }
        public async Task<ResponseModel> AddRangeIngredient(List<IngredientImportModel> ingredients)
        {
            var ingredientList = _mapper.Map<List<Ingredient>>(ingredients);
            if (ingredientList == null || !ingredientList.Any())
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot create Ingredient!"
                };
            }

            var nutrientMapping = ingredients?
                .SelectMany(i => i.Nutrients, (i, n) => new { IngredientName = i.Name, Nutrient = n })?
                .GroupBy(x => x.IngredientName)?
                .ToDictionary(g => g.Key, g => g.Select(x => x.Nutrient).ToList());

            foreach (var ingredient in ingredientList)
            {
                if (nutrientMapping.TryGetValue(ingredient.Name, out var nutrientModels))
                {
                    var nutrients = _mapper.Map<List<Nutrient>>(nutrientModels);
                    ingredient.Nutrients = nutrients;
                }
            }
            await _unitOfWork.IngredientRepository.AddRangeAsync(ingredientList);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Ingredient created successfully"
            };
        }



        public async Task<Pagination<IngredientModel>> GetAllIngredient(IngredientFilterModel ingredientFilterModel)
        {
            var ingredientList = await _unitOfWork.IngredientRepository.GetAllAsync(
                pageIndex: ingredientFilterModel.PageIndex,
                pageSize: ingredientFilterModel.PageSize,
                filter: x =>
                    x.IsDeleted == ingredientFilterModel.IsDeleted &&
                    (!ingredientFilterModel.DishId.HasValue || x.DishIngredients.Any(di => di.DishId == ingredientFilterModel.DishId)) &&
                    (string.IsNullOrEmpty(ingredientFilterModel.Search) ||
                     x.Name.ToLower().Contains(ingredientFilterModel.Search.ToLower())),
                orderBy: x =>
                {
                    switch (ingredientFilterModel.Order.ToLower())
                    {
                        case "name":
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.Name)
                                : x.OrderBy(i => i.Name);
                        case "kcal":
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.Kcal)
                                : x.OrderBy(i => i.Kcal);
                        default:
                            return ingredientFilterModel.OrderByDescending
                                ? x.OrderByDescending(i => i.CreationDate)
                                : x.OrderBy(i => i.CreationDate);
                    }
                },
                include: "DishIngredients,Nutrients"
            );

            if (ingredientList != null)
            {
                var ingredientModelList = _mapper.Map<List<IngredientModel>>(ingredientList.Data);
                return new Pagination<IngredientModel>(
                    ingredientModelList,
                    ingredientList.TotalCount,
                    ingredientFilterModel.PageIndex,
                    ingredientFilterModel.PageSize
                );
            }
            return null;
        }


        public async Task<ResponseModel> UpdateIngredient(Guid id, IngredientUpdateModel updateModel)
        {
            var existingIngredient = await _unitOfWork.IngredientRepository.GetAsync(id, "DishIngredients,Nutrients");
            if (existingIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Ingredient not found!"
                };
            }

            bool isKcalChanged = existingIngredient.Kcal != updateModel.Kcal;
            bool isFatChanged = existingIngredient.Fat != updateModel.Fat;
            bool isProteinChanged = existingIngredient.Protein != updateModel.Protein;
            bool isCarbohydratesChanged = existingIngredient.Carbohydrates != updateModel.Carbohydrates;


            var existingNutrients = existingIngredient.Nutrients;
            var nutrientToUpdate = new List<Nutrient>();

            if (updateModel.Nutrients != null && updateModel.Nutrients.Any())
            {
                foreach (var nutrientUpdate in updateModel.Nutrients)
                {
                    if (nutrientUpdate.Id.HasValue)
                    {
                        var existingNutrient = existingNutrients
                            .FirstOrDefault(n => n.Id == nutrientUpdate.Id.Value);

                        if (existingNutrient != null)
                        {
                            existingNutrient.IngredientId = existingIngredient.Id;
                            _mapper.Map(nutrientUpdate, existingNutrient);
                            nutrientToUpdate.Add(existingNutrient);
                        }
                    }
                }
            }

            // Cập nhật lại Ingredient
            _mapper.Map(updateModel, existingIngredient);

            if (nutrientToUpdate.Any())
            {
                _unitOfWork.NutrientRepository.UpdateRange(nutrientToUpdate);
                await _unitOfWork.SaveChangeAsync();
            }


            if (isKcalChanged || isFatChanged || isProteinChanged || isCarbohydratesChanged)
            {
                // Get affected dishes
                var affectedDishes = await _unitOfWork.DishRepository.GetAllAsync(
                    filter: dish => dish.DishIngredients.Any(di => di.IngredientId == id),
                    include: "DishIngredients.Ingredient"
                );

                // Update dishes
                var updatedDishes = affectedDishes.Data.Select(dish =>
                {
                    dish.Kcal = dish.DishIngredients.Sum(di => di.Ingredient.Kcal);
                    dish.Protein = dish.DishIngredients.Sum(di => di.Ingredient.Protein);
                    dish.Starch = dish.DishIngredients.Sum(di => di.Ingredient.Carbohydrates);
                    dish.Fat = dish.DishIngredients.Sum(di => di.Ingredient.Fat);
                    return dish;
                }).ToList();

                _unitOfWork.DishRepository.UpdateRange(updatedDishes);
                await _unitOfWork.SaveChangeAsync();

                // Get affected meals
                var affectedMealIds = updatedDishes.SelectMany(dish => dish.MealDishes.Select(md => md.MealId)).Distinct();
                var affectedMeals = await _unitOfWork.MealRepository.GetAllAsync(
                    filter: meal => affectedMealIds.Contains(meal.Id),
                    include: "MealDishes.Dish"
                );

                // Update meals
                var updatedMeals = affectedMeals.Data.Select(meal =>
                {
                    meal.Kcal = meal.MealDishes.Sum(md => md.Dish.Kcal);
                    return meal;
                }).ToList();

                _unitOfWork.MealRepository.UpdateRange(updatedMeals);
                await _unitOfWork.SaveChangeAsync();

                // Get affected menus
                var affectedMenuIds = updatedMeals.SelectMany(meal => meal.MenuMeals.Select(mm => mm.MenuId)).Distinct();
                var affectedMenus = await _unitOfWork.MenuRepository.GetAllAsync(
                    filter: menu => affectedMenuIds.Contains(menu.Id),
                    include: "MenuMeals.Meal"
                );

                // Update menus
                var updatedMenus = affectedMenus.Data.Select(menu =>
                {
                    menu.Kcal = menu.MenuMeals.Sum(mm => mm.Meal.Kcal);
                    return menu;
                }).ToList();

                _unitOfWork.MenuRepository.UpdateRange(updatedMenus);
                await _unitOfWork.SaveChangeAsync();
            }

            return new ResponseModel()
            {
                Status = true,
                Message = "Ingredient and related entities updated successfully"
            };
        }


        public async Task<ResponseModel> DeleteIngredient(Guid id)
        {
            var existingIngredient = await _unitOfWork.IngredientRepository.GetAsync(id, "DishIngredients,Nutrients");
            if (existingIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Ingredient not found!"
                };
            }

            _unitOfWork.DishIngredientRepository.HardDeleteRange(existingIngredient.DishIngredients.ToList());
            await _unitOfWork.SaveChangeAsync();
            _unitOfWork.NutrientRepository.HardDeleteRange(existingIngredient.Nutrients.ToList());
            await _unitOfWork.SaveChangeAsync();
            _unitOfWork.IngredientRepository.HardDelete(existingIngredient);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Ingredient deleted successfully"
            };
        }
    }
}
