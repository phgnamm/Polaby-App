using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.MenuModels;
using Polaby.Repositories.Models.MenuModels;
using Polaby.Services.Models.MealModels;
using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Models.DishModels;
using Polaby.Repositories.Models.DishModels;
using Polaby.Services.Models.IngredientModels;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Services.Models.NutrientModels;
using Polaby.Repositories.Models.NutrientModels;

namespace Polaby.Services.Common
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			//Account
			CreateMap<AccountRegisterModel, Account>();
			CreateMap<GoogleUserInformationModel, Account>();
			CreateMap<AccountModel, Account>().ReverseMap();

			//Menu
			CreateMap<MenuImportModel, Menu>().ReverseMap();
            CreateMap<MenuUpdateModel, Menu>();
            CreateMap<MenuModel, Menu>().ReverseMap();

            //MenuMeal
            CreateMap<MenuMealCreateModel, MenuMeal>().ReverseMap();

            //Meal
            CreateMap<MealImportModel, Meal>().ReverseMap();
            CreateMap<MealUpdateModel, Meal>();
            CreateMap<MealModel, Meal>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString())).ReverseMap();

            //Dish
            CreateMap<DishImportModel, Dish>().ReverseMap();
            CreateMap<DishUpdateModel, Dish>();
            CreateMap<DishModel, Dish>().ReverseMap();

            //Ingredient
            CreateMap<IngredientImportModel, Ingredient>().ReverseMap();
            CreateMap<IngredientUpdateModel, Ingredient>();
            CreateMap<IngredientModel, Ingredient>().ReverseMap();

            //Nutrient
            CreateMap<NutrientImportModel, Nutrient>().ReverseMap();
            CreateMap<NutrientUpdateModel, Nutrient>();
            CreateMap<NutrientModel, Nutrient>().ReverseMap();

            //CommunityPost
            CreateMap<CommunityPostCreateModel, CommunityPost>();
        }
	}
}
