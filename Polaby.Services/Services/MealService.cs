using AutoMapper;
using Polaby.Repositories.Entities;
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
                        MealId = addedMeals.Data.First(meal => meal.Name == mealImportModel.Name).Id,
                        DishId = dishId
                    })
                ).ToList();

            await _unitOfWork.MealDishRepository.AddRangeAsync(mealDishes);
            await _unitOfWork.SaveChangeAsync();

            var mealIds = addedMeals.Data.Select(meal => meal.Id).ToList();
            var mealDishesGrouped = await _unitOfWork.MealDishRepository.GetAllAsync(
                filter: md => mealIds.Contains((Guid)md.MealId),
                include: "MealDishes.Dish"
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
                    (string.IsNullOrEmpty(mealFilterModel.Search) ||
                     x.Kcal.Equals(mealFilterModel.Search) ||
                     x.Name.ToString().ToLower().Contains(mealFilterModel.Search.ToLower()))),
                orderBy: x =>
                {
                    switch (mealFilterModel.Order.ToLower())
                    {
                        case "name":
                            return mealFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "kcal":
                            return mealFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Kcal)
                                : x.OrderBy(x => x.Kcal);
                        default:
                            return mealFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                }
            );
            if (mealList != null)
            {
                var mealModelList = _mapper.Map<List<MealModel>>(mealList.Data);
                return new Pagination<MealModel>(mealModelList, mealList.TotalCount, mealFilterModel.PageIndex,
                    mealFilterModel.PageSize);
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
            var newDishIds = updateModel.DishIds.Where(id => !existingDishIds.Contains(id)).ToList();
            var removedDishes = existingDishIds.Where(id => !updateModel.DishIds.Contains((Guid)id)).ToList();

            foreach (var dishId in newDishIds)
            {
                existingMeal.MealDishes.Add(new MealDish { MealId = id, DishId = dishId });
            }

            foreach (var dishId in removedDishes)
            {
                var mealDish = existingMeal.MealDishes.FirstOrDefault(md => md.DishId == dishId);
                if (mealDish != null)
                {
                    existingMeal.MealDishes.Remove(mealDish);
                }
            }
            await _unitOfWork.SaveChangeAsync();

            var updatedMealDishes = await _unitOfWork.MealDishRepository.GetAllAsync(
                filter: md => md.MealId == id,
                include: "MealDishes.Dish"
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
    }
}
