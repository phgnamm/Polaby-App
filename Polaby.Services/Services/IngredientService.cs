
using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.IngredientModels;
using Polaby.Services.Models.NutrientModels;
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

            foreach (var ingredient in ingredientList)
            {
                var nutrientModels = ingredients
                    .FirstOrDefault(i => i.Name == ingredient.Name)?
                    .Nutrients;

                if (nutrientModels != null)
                {
                    var nutrients = _mapper.Map<List<Nutrient>>(nutrientModels);
                    foreach (var nutrient in nutrients)
                    {
                        ingredient.Nutrients.Add(nutrient);
                    }
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
                include: "DishIngredients.Ingredient,Nutrients"
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

            _mapper.Map(updateModel, existingIngredient);
            existingIngredient.Nutrients.Clear();
            if (updateModel.Nutrients != null && updateModel.Nutrients.Any())
            {
                var updatedNutrients = _mapper.Map<List<Nutrient>>(updateModel.Nutrients);
                foreach (var nutrient in updatedNutrients)
                {
                    existingIngredient.Nutrients.Add(nutrient);
                }
            }
            await _unitOfWork.SaveChangeAsync();

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
            var existingIngredient = await _unitOfWork.IngredientRepository.GetAsync(id, "DishIngredients");
            if (existingIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Ingredient not found!"
                };
            }

            _unitOfWork.NutrientRepository.HardDeleteRange(existingIngredient.Nutrients.ToList());
            _unitOfWork.DishIngredientRepository.HardDeleteRange(existingIngredient.DishIngredients.ToList());
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
