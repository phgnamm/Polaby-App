using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.MealModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class MealService : IMealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public MealService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }
        public async Task<ResponseModel> AddRangeMeal(List<MealImportModel> meals)
        {
            var mealList = _mapper.Map<List<Meal>>(meals);

            if (mealList == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot create Meal!"
                };
            }

            await _unitOfWork.MealRepository.AddRangeAsync(mealList);
            await _unitOfWork.SaveChangeAsync();

            var addedMeals = await _unitOfWork.MealRepository.GetAllAsync(
                filter: meal => meals.Select(m => m.Name).Contains(meal.Name)
            );

            var mealDishes = meals
                .SelectMany(mealImportModel =>
                    mealImportModel.DishIds.Select(dishId => new MealDish
                    {
                        MealId = addedMeals.Data.Last(meal => meal.Name == mealImportModel.Name).Id,
                        DishId = dishId
                    })
                ).ToList();

            await _unitOfWork.MealDishRepository.AddRangeAsync(mealDishes);
            await _unitOfWork.SaveChangeAsync();

            var mealIds = addedMeals.Data.Select(meal => meal.Id).ToList();
            var mealDishesGrouped = await _unitOfWork.MealDishRepository.GetAllAsync(
                filter: md => mealIds.Contains((Guid)md.MealId),
                include: "Meal,Dish"
            );

            var kcalUpdates = mealDishesGrouped.Data
                .GroupBy(md => md.MealId)
                .Select(group => new
                {
                    MealId = group.Key,
                    TotalKcal = group.Sum(md => md.Dish.Kcal)
                }).ToList();

            var updatedMeals = addedMeals.Data
                .Join(kcalUpdates,
                    meal => meal.Id,
                    update => update.MealId,
                    (meal, update) =>
                    {
                        meal.Kcal = update.TotalKcal;
                        return meal;
                    }).ToList();

            _unitOfWork.MealRepository.UpdateRange(updatedMeals);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Meal created successfully with associated dishes and updated Kcal"
            };
        }


        public async Task<Pagination<MealModel>> GetAllMeal(MealFilterModel mealFilterModel)
        {
            var mealList = await _unitOfWork.MealRepository.GetAllAsync(pageIndex: mealFilterModel.PageIndex,
                pageSize: mealFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == mealFilterModel.IsDeleted &&
                    (!mealFilterModel.MenuId.HasValue || x.MenuMeals.Any(mm => mm.MenuId == mealFilterModel.MenuId)) &&
                    (string.IsNullOrEmpty(mealFilterModel.Search) ||
                     x.Kcal.Equals(mealFilterModel.Search) ||
                     x.Name.ToString().ToLower().Contains(mealFilterModel.Search.ToLower()))),
                orderBy: x =>
                {
                    switch (mealFilterModel.Order.ToLower())
                    {
                        case "name":
                            return mealFilterModel.OrderByDescending
                                ? x.OrderBy(x => x.Name.HasValue ? (int)x.Name.Value : int.MaxValue)
                                : x.OrderByDescending(x => x.Name.HasValue ? (int)x.Name.Value : int.MinValue);
                        case "kcal":
                            return mealFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Kcal)
                                : x.OrderBy(x => x.Kcal);
                        default:
                            return mealFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                },
                include: "MenuMeals"
            );
            if (mealList != null)
            {
                var mealModelList = _mapper.Map<List<MealModel>>(mealList.Data);
                return new Pagination<MealModel>(mealModelList, mealFilterModel.PageIndex,
                    mealFilterModel.PageSize, mealList.TotalCount);
            }
            return null;
        }

        public async Task<ResponseModel> UpdateMeal(Guid id, MealUpdateModel updateModel)
        {
            var existingMeal = await _unitOfWork.MealRepository.GetAsync(id, "MealDishes");
            if (existingMeal == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Meal not found!"
                };
            }

            _mapper.Map(updateModel, existingMeal);
            var existingDishIds = existingMeal.MealDishes.Select(md => md.DishId).ToList();
            foreach (var dishId in updateModel.DishIds)
            {
                if (!existingDishIds.Contains(dishId))
                {
                    existingMeal.MealDishes.Add(new MealDish { MealId = id, DishId = dishId });
                }
            }
            await _unitOfWork.SaveChangeAsync();

            var updatedMealDishes = await _unitOfWork.MealDishRepository.GetAllAsync(
                filter: md => md.MealId == id,
                include: "Dish"
            );
            var totalKcal = updatedMealDishes.Data.Sum(md => md.Dish.Kcal);
            existingMeal.Kcal = totalKcal;
            _unitOfWork.MealRepository.Update(existingMeal);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Meal updated successfully with updated Kcal"
            };
        }



        public async Task<ResponseModel> DeleteMeal(Guid id)
        {
            var existingMeal = await _unitOfWork.MealRepository.GetAsync(id, "MealDishes");
            if (existingMeal == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Meal not found!"
                };
            }

            var mealDishToRemove = existingMeal.MealDishes.ToList();
            _unitOfWork.MealDishRepository.HardDeleteRange(mealDishToRemove);
            _unitOfWork.MealRepository.HardDelete(existingMeal);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Meal deleted successfully"
            };
        }
        public async Task<ResponseModel> DeleteMealDish(Guid mealId, Guid dishId)
        {
            var existingMealDish = await _unitOfWork.MealDishRepository.GetMealDishesAsync(mealId, dishId);
            if (existingMealDish == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "MealDish not found!"
                };
            }

            _unitOfWork.MealDishRepository.HardDeleteRange(existingMealDish);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "MealDish deleted successfully"
            };
        }

        public async Task<ResponseDataModel<MealModel>> GetById(Guid id)
        {
            var meal = await _unitOfWork.MealRepository.GetAsync(id);

            if (meal == null)
            {
                return new ResponseDataModel<MealModel>()
                {
                    Status = false,
                    Message = "Meal not found"
                };
            }

            var mealModel = _mapper.Map<MealModel>(meal);

            return new ResponseDataModel<MealModel>()
            {
                Status = true,
                Message = "Get meal successfully",
                Data = mealModel
            };
        }
    }
}
