
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
            if (dishes == null || !dishes.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No dishes provided!"
                };
            }

            var dishList = _mapper.Map<List<Dish>>(dishes);
            await _unitOfWork.DishRepository.AddRangeAsync(dishList);
            await _unitOfWork.SaveChangeAsync();

            var addedDishes = await _unitOfWork.DishRepository.GetAllAsync(
                filter: dish => dishes.Select(d => d.Name).Contains(dish.Name)
            );

            var dishIngredients = dishes
                .SelectMany(dishImportModel => dishImportModel.IngredientIds.Select(ingredientId => new DishIngredient
                {
                    DishId = addedDishes.Data.First(dish => dish.Name == dishImportModel.Name).Id,
                    IngredientId = ingredientId
                })).ToList();

            var dishNutrients = dishes
                .Where(dishImportModel => dishImportModel.Nutrients != null && dishImportModel.Nutrients.Any())
                .SelectMany(dishImportModel => _mapper.Map<List<Nutrient>>(dishImportModel.Nutrients)
                    .Select(nutrient =>
                    {
                        nutrient.DishId = addedDishes.Data.First(dish => dish.Name == dishImportModel.Name).Id;
                        return nutrient;
                    })).ToList();

            if (dishIngredients.Any())
            {
                await _unitOfWork.DishIngredientRepository.AddRangeAsync(dishIngredients);
            }

            if (dishNutrients.Any())
            {
                await _unitOfWork.NutrientRepository.AddRangeAsync(dishNutrients);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Dishes created successfully with associated Ingredients and Nutrients"
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
                include: "DishIngredients,Nutrients"
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
            var existingDish = await _unitOfWork.DishRepository.GetAsync(id, "DishIngredients,Nutrients");
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
                include: "DishIngredients,Ingredient,Nutrient"
            );
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
            var existingDish = await _unitOfWork.DishRepository.GetAsync(id, "DishIngredients,Nutrients");
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
            _unitOfWork.NutrientRepository.HardDeleteRange(existingDish.Nutrients.ToList());
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
