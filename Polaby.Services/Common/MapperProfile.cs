using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.MenuModels;
using Polaby.Repositories.Models.MenuModels;

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
        }

            //CommunityPost
            CreateMap<CommunityPostCreateModel, CommunityPost>();
        }
	}
}
