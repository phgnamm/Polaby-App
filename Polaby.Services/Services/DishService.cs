
using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.DishModels;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.DishModels;
using Polaby.Services.Models.MenuModels;
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

        public async Task<ResponseModel> AddDishIngredient(DishIngredientCreateModel model)
        {
            var distinctIngredientIds = model.IngredientIds.Distinct().ToList();
            if (!distinctIngredientIds.Any())
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "No valid ingredients provided!"
                };
            }

            var dishIngredients = distinctIngredientIds.Select(ingredientId => new DishIngredient
            {
                DishId = model.DishId,
                IngredientId = ingredientId
            }).ToList();

            await _unitOfWork.DishIngredientRepository.AddRangeAsync(dishIngredients);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "DishIngredients created successfully"
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
                include: "Nutrients,DishIngredients.Ingredient"
            );
            if (dishList != null)
            {
                var dishModelList = _mapper.Map<List<DishModel>>(dishList.Data);

                foreach (var dishModel in dishModelList)
                {
                    var dishEntity = dishList.Data.FirstOrDefault(d => d.Id == dishModel.Id);
                    if (dishEntity != null && dishEntity.DishIngredients != null)
                    {
                        dishModel.Ingredients = dishEntity.DishIngredients
                            .Select(di => _mapper.Map<IngredientModel>(di.Ingredient))
                            .ToList();
                    }
                }

                return new Pagination<DishModel>(dishModelList, dishFilterModel.PageIndex,
                    dishFilterModel.PageSize, dishList.TotalCount);
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

            var existingNutrients = existingDish.Nutrients;
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
                            existingNutrient.DishId = existingDish.Id;
                            _mapper.Map(nutrientUpdate, existingNutrient);
                            nutrientToUpdate.Add(existingNutrient);
                        }
                    }
                }
            } 

            _mapper.Map(updateModel, existingDish);
            if (nutrientToUpdate.Any())
            {
                _unitOfWork.NutrientRepository.UpdateRange(nutrientToUpdate);
                await _unitOfWork.SaveChangeAsync();
            }

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

        public async Task<ResponseModel> DeleteDishIngredient(Guid dishId, Guid ingredientId)
        {
            var existingDishIngredient = await _unitOfWork.DishIngredientRepository.GetDishIngredientsAsync(dishId,ingredientId);
            if (existingDishIngredient == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "DishIngredient not found!"
                };
            }

            _unitOfWork.DishIngredientRepository.HardDeleteRange(existingDishIngredient);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "DishIngredient deleted successfully"
            };
        }

        public async Task<ResponseDataModel<DishModel>> GetById(Guid id)
        {
            var dish = await _unitOfWork.DishRepository.GetAsync(id, "DishIngredients.Ingredient,Nutrients");
            if (dish == null)
            {
                return new ResponseDataModel<DishModel>()
                {
                    Status = false,
                    Message = "Dish not found"
                };
            }

            var dishModel = _mapper.Map<DishModel>(dish);
            if (dish.DishIngredients != null)
            {
                dishModel.Ingredients = dish.DishIngredients
                    .Select(di => _mapper.Map<IngredientModel>(di.Ingredient))
                    .ToList();
            }

            return new ResponseDataModel<DishModel>()
            {
                Status = true,
                Message = "Get dish successfully",
                Data = dishModel
            };
        }

    }
}
