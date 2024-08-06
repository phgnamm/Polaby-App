using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.CommunityPostModels;

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

            //CommunityPost
            CreateMap<CommunityPostCreateModel, CommunityPost>();
        }
	}
}
