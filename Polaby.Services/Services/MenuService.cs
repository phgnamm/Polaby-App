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
using Polaby.Services.Models.UserMenuModels;
using Polaby.Repositories.Models.IngredientModels;

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
            if (menus == null || !menus.Any())
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "No menus provided!"
                };
            }

            var menuList = _mapper.Map<List<Menu>>(menus);
            await _unitOfWork.MenuRepository.AddRangeAsync(menuList);
            await _unitOfWork.SaveChangeAsync();

            var addedMenus = await _unitOfWork.MenuRepository.GetAllAsync(
                filter: menu => menus.Select(m => m.Name).Contains(menu.Name)
            );

            var menuMeals = menus
                .SelectMany(menuImportModel =>
                    menuImportModel.MealIds.Select(mealId => new MenuMeal
                    {
                        MenuId = addedMenus.Data.FirstOrDefault(menu => menu.Name == menuImportModel.Name)?.Id,
                        MealId = mealId
                    })
                ).ToList();

            var menuNutrients = menus
                .Where(menuImportModel => menuImportModel.Nutrients != null && menuImportModel.Nutrients.Any())
                .SelectMany(menuImportModel => _mapper.Map<List<Nutrient>>(menuImportModel.Nutrients)
                    .Select(nutrient =>
                    {
                        nutrient.MenuId = addedMenus.Data.FirstOrDefault(menu => menu.Name == menuImportModel.Name)?.Id;
                        return nutrient;
                    })).ToList();

            if (menuMeals.Any())
            {
                await _unitOfWork.MenuMealRepository.AddRangeAsync(menuMeals);
            }

            if (menuNutrients.Any())
            {
                await _unitOfWork.NutrientRepository.AddRangeAsync(menuNutrients);
            }

            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Menu created successfully with associated meals and nutrients"
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
                        default:
                            return menuFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                },
                 include: "Nutrients"
            );
            if (menuList != null)
            {
                var menuModelList = _mapper.Map<List<MenuModel>>(menuList.Data);
                return new Pagination<MenuModel>(menuModelList, menuFilterModel.PageIndex,
                    menuFilterModel.PageSize, menuList.TotalCount);
            }
            return null;
        }

        public async Task<ResponseModel> UpdateMenu(Guid id, MenuUpdateModel updateModel)
        {
            var existingMenu = await _unitOfWork.MenuRepository.GetAsync(id, "MenuMeals,Nutrients");
            if (existingMenu == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Menu not found!"
                };
            }

            _mapper.Map(updateModel, existingMenu);
            var existingMealIds = existingMenu.MenuMeals.Select(mm => mm.MealId).ToList();
            foreach (var mealId in updateModel.MealIds)
            {
                if (!existingMealIds.Contains(mealId))
                {
                    existingMenu.MenuMeals.Add(new MenuMeal { MenuId = id, MealId = mealId });
                }
            }

            var existingNutrients = existingMenu.Nutrients;
            var nutrientsToUpdate = new List<Nutrient>();

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
                            _mapper.Map(nutrientUpdate, existingNutrient);
                            nutrientsToUpdate.Add(existingNutrient);
                        }
                    }
                    else
                    {
                        var newNutrient = _mapper.Map<Nutrient>(nutrientUpdate);
                        newNutrient.MenuId = id;
                        existingMenu.Nutrients.Add(newNutrient);
                    }
                }
            }

            _unitOfWork.MenuRepository.Update(existingMenu);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Menu updated successfully with associated Nutrients"
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
            var menuList = menuMeals.SelectMany(menuMeal => _mapper.Map<List<MenuMeal>>(menuMeal)).ToList();
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

        public async Task<ResponseModel> DeleteMenuMeal(Guid menuId, Guid mealId)
        {
            var existingMenuMeal = await _unitOfWork.MenuMealRepository.GetMenuMealsAsync(menuId, mealId);
            if (existingMenuMeal == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "MenuMeal not found!"
                };
            }

            _unitOfWork.MenuMealRepository.HardDeleteRange(existingMenuMeal);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "MenuMeal deleted successfully"
            };
        }

        public async Task<ResponseModel> AddUserMenu(UserMenuMCreateModel model)
        {
            if (model == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "No UserMenus provided!"
                };
            }
            var userMenu = new UserMenu
            {
                UserId = model.UserId.Value,
                MenuId = model.MenuId.Value 
            };

            await _unitOfWork.UserMenuRepository.AddAsync(userMenu);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "Menu saved successfully!"
            };
        }

        public async Task<ResponseDataModel<List<Menu>>> GetAllUserMenuAsync(Guid userId)
        {
            var userMenus = await _unitOfWork.UserMenuRepository.GetAllAsync(
                filter: um => um.UserId == userId,
                include: "Menu"
            );

            if (userMenus == null || !userMenus.Data.Any())
            {
                return new ResponseDataModel<List<Menu>>()
                {
                    Status = false,
                    Message = "No menus found for this user."
                };
            }

            var menus = userMenus.Data.Select(um => um.Menu).ToList();

            return new ResponseDataModel<List<Menu>>()
            {
                Status = true,
                Data = menus
            };
        }

        public async Task<ResponseModel> DeleteUserMenu(Guid userId, Guid menuId)
        {
            var existingUserMenu = await _unitOfWork.UserMenuRepository.GetUserMenusAsync(userId, menuId);
            if (existingUserMenu == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "UserMenu not found!"
                };
            }

            _unitOfWork.UserMenuRepository.HardDeleteRange(existingUserMenu);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Status = true,
                Message = "UserMenu deleted successfully"
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
                    menu.Kcal <= maxCalories,
                orderBy: menus => menus.OrderBy(menu => menu.Name),
                include: "MenuMeals,MenuMeals.Meal,MenuMeals.Meal.MealDishes,MenuMeals.Meal.MealDishes.Dish,MenuMeals.Meal.MealDishes.Dish.DishIngredients,MenuMeals.Meal.MealDishes.Dish.DishIngredients.Ingredient,Nutrients"
 );


            var menus = queryResult.Data
        .Where(menu =>
            menu.MenuMeals.All(menuMeal =>
                menuMeal.Meal.MealDishes.All(mealDish =>
                    IsDietSuitable(mealDish.Dish, account.Diet)
                )
            )
        )
        .Select(menu =>
        {
            var menuModel = _mapper.Map<MenuModel>(menu);
            menuModel.KcalRecomment = totalCaloriesRequired;
            menuModel.MealCount = menu.MenuMeals.Count; // Count the number of meals
            return menuModel;
        })
        .ToList();

            return new Pagination<MenuModel>(menus, queryResult.TotalCount, model.PageIndex, model.PageSize);
        }



        private int CalculateTotalCaloriesRequired(AccountModel account)
        {
            int baseCalories = 0;
            DateTime dueDate = account.DueDate.Value.ToDateTime(TimeOnly.MinValue);
            DateTime startOfPregnancy = dueDate.AddDays(-280);
            DateTime currentDate = DateTime.Now.Date;
            int currentWeek = (int)((currentDate - startOfPregnancy).TotalDays / 7);

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

        public async Task<ResponseDataModel<MenuModel>> GetById(Guid id)
        {
            var menu = await _unitOfWork.MenuRepository.GetById(id);

            if (menu == null)
            {
                return new ResponseDataModel<MenuModel>()
                {
                    Status = false,
                    Message = "Menu not found"
                };
            }

            var menuModel = _mapper.Map<MenuModel>(menu);

            return new ResponseDataModel<MenuModel>()
            {
                Status = true,
                Message = "Get menu successfully",
                Data = menuModel
            };
        }
    }
}
