
using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.DishModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.DishModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class DishService : IDishService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public DishService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<ResponseModel> AddRangeDish(List<DishImportModel> dishes)
        {
            var dishList = _mapper.Map<List<Dish>>(dishes);

            if (dishList == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot create Dish!"
                };
            }

            await _unitOfWork.DishRepository.AddRangeAsync(dishList);
            await _unitOfWork.SaveChangeAsync();

            var addedDishes = await _unitOfWork.DishRepository.GetAllAsync(
                filter: dish => dishes.Select(d => d.Name).Contains(dish.Name)
            );

            var dishIngredients = dishes
                .SelectMany(dishImportModel =>
                    dishImportModel.IngredientIds.Select(IngredientId => new DishIngredient
                    {
                        DishId = addedDishes.Data.First(dish => dish.Name == dishImportModel.Name).Id,
                        IngredientId = IngredientId
                    })
                ).ToList();

            await _unitOfWork.DishIngredientRepository.AddRangeAsync(dishIngredients);
            await _unitOfWork.SaveChangeAsync();

            var dishIds = addedDishes.Data.Select(dish => dish.Id).ToList();
            var dishIngredientsGrouped = await _unitOfWork.DishIngredientRepository.GetAllAsync(
                filter: di => dishIds.Contains((Guid)di.DishId),
                include: "DishIngredients.Ingredient"
            );

            var kcalUpdates = dishIngredientsGrouped.Data
       .GroupBy(di => di.DishId)
       .Select(group => new
       {
           DishId = group.Key,
           TotalKcal = group.Sum(di => di.Ingredient.Kcal),
           TotalProtein = group.Sum(di => di.Ingredient.Protein),
           TotalStarch = group.Sum(di => di.Ingredient.Carbohydrates),
           TotalFat = group.Sum(di => di.Ingredient.Fat)
       }).ToList();

            var updatedDishes = addedDishes.Data
                .Join(kcalUpdates,
                    dish => dish.Id,
                    update => update.DishId,
                    (dish, update) =>
                    {
                        dish.Kcal = update.TotalKcal;
                        dish.Protein = update.TotalProtein;
                        dish.Starch = update.TotalStarch;
                        dish.Fat = update.TotalFat;
                        return dish;
                    }).ToList();

            _unitOfWork.DishRepository.UpdateRange(updatedDishes);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Dish created successfully with associated Ingredients and updated Kcal"
            };
        }


        public async Task<Pagination<DishModel>> GetAllDish(DishFilterModel dishFilterModel)
        {
            var dishList = await _unitOfWork.DishRepository.GetAllAsync(pageIndex: dishFilterModel.PageIndex,
                pageSize: dishFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == dishFilterModel.IsDeleted &&
                    (!dishFilterModel.MealId.HasValue || x.MealDishes.Any(mm => mm.MealId == dishFilterModel.MealId)) &&
                    (string.IsNullOrEmpty(dishFilterModel.Search) ||
                     x.Kcal.Equals(dishFilterModel.Search) ||
                     x.Name.ToString().ToLower().Contains(dishFilterModel.Search.ToLower()))),
                orderBy: x =>
                {
                    switch (dishFilterModel.Order.ToLower())
                    {
                        case "name":
                            return dishFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "kcal":
                            return dishFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Kcal)
                                : x.OrderBy(x => x.Kcal);
                        default:
                            return dishFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                },
                include: "DishIngredients"
            );
            if (dishList != null)
            {
                var mealModelList = _mapper.Map<List<DishModel>>(dishList.Data);
                return new Pagination<DishModel>(mealModelList, dishList.TotalCount, dishFilterModel.PageIndex,
                    dishFilterModel.PageSize);
            }
            return null;
        }

        public async Task<ResponseModel> UpdateDish(Guid id, DishUpdateModel updateModel)
        {
            var existingDish = await _unitOfWork.DishRepository.GetAsync(id, "DishIngredients");
            if (existingDish == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Dish not found!"
                };
            }

            _mapper.Map(updateModel, existingDish);

            var existingIngredientIds = existingDish.MealDishes.Select(md => md.DishId).ToList();
            var newIngredientIds = updateModel.IngredientIds.Where(id => !existingIngredientIds.Contains(id)).ToList();
            var removedIngredients = existingIngredientIds.Where(id => !updateModel.IngredientIds.Contains((Guid)id)).ToList();

            foreach (var ingredientId in newIngredientIds)
            {
                existingDish.DishIngredients.Add(new DishIngredient { DishId = id, IngredientId = ingredientId });
            }

            foreach (var ingredientId in removedIngredients)
            {
                var dishIngredient = existingDish.DishIngredients.FirstOrDefault(di => di.IngredientId == ingredientId);
                if (dishIngredient != null)
                {
                    existingDish.DishIngredients.Remove(dishIngredient);
                }
            }
            await _unitOfWork.SaveChangeAsync();

            var updatedDishIngredient = await _unitOfWork.DishIngredientRepository.GetAllAsync(
                filter: di => di.IngredientId == id,
                include: "DishIngredients.Ingredient"
            );

            var totalKcal = updatedDishIngredient.Data.Sum(di => di.Ingredient.Kcal);
            var totalProtein = updatedDishIngredient.Data.Sum(di => di.Ingredient.Protein);
            var totalStarch = updatedDishIngredient.Data.Sum(di => di.Ingredient.Carbohydrates);
            var totalFat = updatedDishIngredient.Data.Sum(di => di.Ingredient.Fat);

            existingDish.Kcal = totalKcal;
            existingDish.Protein = totalProtein;
            existingDish.Starch = totalStarch;
            existingDish.Fat = totalFat;

            _unitOfWork.DishRepository.Update(existingDish);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Dish updated successfully with updated Kcal"
            };
        }


        public async Task<ResponseModel> DeleteDish(Guid id)
        {
            var existingDish = await _unitOfWork.DishRepository.GetAsync(id, "DishIngredients");
            if (existingDish == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Dish not found!"
                };
            }

            var dishIngredientToRemove = existingDish.DishIngredients.ToList();
            _unitOfWork.DishIngredientRepository.HardDeleteRange(dishIngredientToRemove);
            _unitOfWork.DishRepository.HardDelete(existingDish);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Dish deleted successfully"
            };
        }
    }
}
