using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.MenuModels;
using Polaby.Repositories.Models.MenuModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IAccountService _accountService;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _accountService = accountService;
        }

        public async Task<ResponseModel> AddRangeMenu(List<MenuImportModel> menus)
        {
            var menuList = _mapper.Map<List<Menu>>(menus);
            if (menuList == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot create Menu!"
                };
            }
            await _unitOfWork.MenuRepository.AddRangeAsync(menuList);
            await _unitOfWork.SaveChangeAsync();

            var addedMenus = await _unitOfWork.MenuRepository.GetAllAsync(
                filter: menu => menus.Select(m => m.Name).Contains(menu.Name)
            );

            var menuMeals = menus
                .SelectMany(menuImportModel =>
                    menuImportModel.MealIds.Select(mealId => new MenuMeal
                    {
                        MenuId = addedMenus.Data.First(menu => menu.Name == menuImportModel.Name).Id,
                        MealId = mealId
                    })
                ).ToList();

            await _unitOfWork.MenuMealRepository.AddRangeAsync(menuMeals);
            await _unitOfWork.SaveChangeAsync();

            var menuIds = addedMenus.Data.Select(menu => menu.Id).ToList();
            var menuMealsGrouped = await _unitOfWork.MenuMealRepository.GetAllAsync(
                filter: mm => menuIds.Contains((Guid)mm.MenuId),
                include: "MenuMeals.Meal" 
            );

            var kcalUpdates = menuMealsGrouped.Data
                .GroupBy(mm => mm.MenuId)
                .Select(group => new
                {
                    MenuId = group.Key,
                    TotalKcal = group.Sum(mm => mm.Meal.Kcal)
                }).ToList();

            var updatedMenus = addedMenus.Data
                .Join(kcalUpdates,
                    menu => menu.Id,
                    update => update.MenuId,
                    (menu, update) =>
                    {
                        menu.Kcal = update.TotalKcal;
                        return menu;
                    }).ToList();

            _unitOfWork.MenuRepository.UpdateRange(updatedMenus);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Menu created successfully with associated meals and updated Kcal"
            };
        }



        public async Task<Pagination<MenuModel>> GetAllMenu(MenuFilterModel menuFilterModel)
        {
            var menuList = await _unitOfWork.MenuRepository.GetAllAsync(pageIndex: menuFilterModel.PageIndex,
                pageSize: menuFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == menuFilterModel.IsDeleted &&
                    menuFilterModel.KcalValues.Any(kcalValue => x.Kcal >= kcalValue - 50 && x.Kcal <= kcalValue + 50) &&
                    (string.IsNullOrEmpty(menuFilterModel.Search) ||
                     x.Kcal.Equals(menuFilterModel.Search) ||
                     x.Water.Equals(menuFilterModel.Search) ||
                     x.Description.ToLower().Contains(menuFilterModel.Search.ToLower()) ||
                     x.Name.ToLower().Contains(menuFilterModel.Search.ToLower()))),
                orderBy: x =>
                {
                    switch (menuFilterModel.Order.ToLower())
                    {
                        case "name":
                            return menuFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "kcal":
                            return menuFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Kcal)
                                : x.OrderBy(x => x.Kcal);
                        case "water":
                            return menuFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Water)
                                : x.OrderBy(x => x.Water);
                        default:
                            return menuFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                }
            );
            if (menuList != null)
            {
                var menuModelList = _mapper.Map<List<MenuModel>>(menuList.Data);
                return new Pagination<MenuModel>(menuModelList, menuList.TotalCount, menuFilterModel.PageIndex,
                    menuFilterModel.PageSize);
            }
            return null;
        }

        public async Task<ResponseModel> UpdateMenu(Guid id, MenuUpdateModel updateModel)
        {
            var existingMenu = await _unitOfWork.MenuRepository.GetAsync(id, "MenuMeals");
            if (existingMenu == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Menu not found!"
                };
            }
            _mapper.Map(updateModel, existingMenu);

            var existingMealIds = existingMenu.MenuMeals.Select(mm => mm.MealId.Value).ToList();
            var newMealIds = updateModel.MealIds.Where(id => !existingMealIds.Contains(id)).ToList();
            var removedMealIds = existingMealIds.Where(id => !updateModel.MealIds.Contains(id)).ToList();

            foreach (var mealId in newMealIds)
            {
                existingMenu.MenuMeals.Add(new MenuMeal { MealId = mealId });
            }

            foreach (var mealId in removedMealIds)
            {
                var menuMeal = existingMenu.MenuMeals.FirstOrDefault(mm => mm.MealId == mealId);
                if (menuMeal != null)
                {
                    existingMenu.MenuMeals.Remove(menuMeal);
                }
            }

            var updatedMenuMeals = await _unitOfWork.MenuMealRepository.GetAllAsync(
                filter: mm => mm.MenuId == id,
                include: "MenuMeals.Meal"
            );

            var totalKcal = updatedMenuMeals.Data.Sum(mm => mm.Meal.Kcal);
            existingMenu.Kcal = totalKcal;

            _unitOfWork.MenuRepository.Update(existingMenu);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Menu updated successfully"
            };
        }

        public async Task<ResponseModel> DeleteMenu(Guid id)
        {
            var existingMenu = await _unitOfWork.MenuRepository.GetAsync(id, "MenuMeals");
            if (existingMenu == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Menu not found!"
                };
            }

            var menuMealsToRemove = existingMenu.MenuMeals.ToList();
            _unitOfWork.MenuMealRepository.HardDeleteRange(menuMealsToRemove);
            _unitOfWork.MenuRepository.HardDelete(existingMenu);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Menu deleted successfully"
            };
        }

        public async Task<ResponseModel> AddRangeMenuMeal(List<MenuMealCreateModel> menuMeals)
        {
            var menuList = _mapper.Map<List<MenuMeal>>(menuMeals);
            if (menuList != null)
            {
                await _unitOfWork.MenuMealRepository.AddRangeAsync(menuList);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "MenuMeal created successfully"
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Can not create MenuMeal !"
            };
        }
        public async Task<Pagination<MenuModel>> GetMenuRecommendations(MenuRecommentFilterModel model)
        {
            var accountResponse = await _accountService.GetAccount(model.AccountId.Value);
            if (accountResponse == null || accountResponse.Data == null)
            {
                throw new Exception("Account not found");
            }

            var account = accountResponse.Data;
            int totalCaloriesRequired = CalculateTotalCaloriesRequired(account);
            int minCalories = totalCaloriesRequired - 50;
            int maxCalories = totalCaloriesRequired + 50;

            var queryResult = await _unitOfWork.MenuRepository.GetAllAsync(
              pageIndex: model.PageIndex,
                pageSize: model.PageSize,
                filter: menu =>
                    menu.Kcal >= minCalories &&
                    menu.Kcal <= maxCalories &&
                    menu.MenuMeals.All(menuMeal =>
                        menuMeal.Meal.MealDishes.All(mealDish => IsDietSuitable(mealDish.Dish, account.Diet))),
                orderBy: menus => menus.OrderBy(menu => menu.Name),
                include: "MenuMeals.Meal.MealDishes.Dish.DishIngredients.Ingredient"
            );
            var menus = _mapper.Map<List<MenuModel>>(queryResult.Data);
            return new Pagination<MenuModel>(menus, queryResult.TotalCount, model.PageIndex, model.PageSize);
        }



        private int CalculateTotalCaloriesRequired(AccountModel account)
        {
            int baseCalories = 0;
            int currentWeek = 41 - ((DateTime.Now.Date - account.DueDate.Value.ToDateTime(TimeOnly.MinValue)).Days / 7);
            switch (account.BMI)
            {
                case BMI.Underweight:
                    baseCalories = account.FrequencyOfActivity switch
                    {
                        FrequencyOfActivity.Light => currentWeek <= 12 ? 1810 :
                                                       currentWeek <= 27 ? 2010 : 2210,
                        FrequencyOfActivity.Moderate => currentWeek <= 12 ? 2100 :
                                                          currentWeek <= 27 ? 2300 : 2500,
                        FrequencyOfActivity.Intense => currentWeek <= 12 ? 2390 :
                                                         currentWeek <= 27 ? 2590 : 2790,
                        _ => throw new ArgumentException("Invalid activity level.")
                    };
                    break;

                case BMI.NormalWeight:
                    baseCalories = account.FrequencyOfActivity switch
                    {
                        FrequencyOfActivity.Light => currentWeek <= 12 ? 1810 :
                                                       currentWeek <= 27 ? 2010 : 2210,
                        FrequencyOfActivity.Moderate => currentWeek <= 12 ? 2100 :
                                                          currentWeek <= 27 ? 2300 : 2500,
                        FrequencyOfActivity.Intense => currentWeek <= 12 ? 2390 :
                                                         currentWeek <= 27 ? 2590 : 2790,
                        _ => throw new ArgumentException("Invalid activity level.")
                    };
                    break;

                case BMI.Overweight:
                    baseCalories = account.FrequencyOfActivity switch
                    {
                        FrequencyOfActivity.Light => currentWeek <= 12 ? 1760 :
                                                       currentWeek <= 27 ? 1960 : 2160,
                        FrequencyOfActivity.Moderate => currentWeek <= 12 ? 2050 :
                                                          currentWeek <= 27 ? 2250 : 2450,
                        FrequencyOfActivity.Intense => currentWeek <= 12 ? 2340 :
                                                         currentWeek <= 27 ? 2540 : 2740,
                        _ => throw new ArgumentException("Invalid activity level.")
                    };
                    break;

                default:
                    throw new ArgumentException("Invalid BMI category.");
            }

            return baseCalories;
        }


        private bool IsDietSuitable(Dish dish, Diet? diet)
        {
            if (diet == null)
            {
                return true;
            }

            switch (diet)
            {
                case Diet.NoAnimalProducts:
                    return !dish.DishIngredients.Any(di => di.Ingredient.Animal);

                case Diet.ModerateAnimalProducts:
                    return dish.DishIngredients.Count(di => di.Ingredient.Animal) <= 2;

                case Diet.HighAnimalProducts:
                    return true;

                default:
                    return true;
            }
        }
    }
}
