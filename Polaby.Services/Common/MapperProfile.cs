using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Repositories.Models.ReportModels;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.FollowModels;
using Polaby.Services.Models.ScheduleModels;
using Polaby.Services.Models.ReportModels;

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

            //Report
            CreateMap<ReportCreateModel, Report>();
            CreateMap<ReportModel, Report>().ReverseMap();

            //CommunityPost
            CreateMap<CommunityPostCreateModel, CommunityPost>();
            CreateMap<CommunityPost, CommunityPostModel>();
            CreateMap<CommunityPostUpdateModel, CommunityPost>();

            //Comment
            CreateMap<CommentCreateModel, Comment>();
            CreateMap<Comment, CommentModel>();
            CreateMap<CommentUpdateModel, Comment>();

            //Schedule
            CreateMap<ScheduleCreateModel, Schedule>();
            CreateMap<Schedule, ScheduleModel>();
            CreateMap<ScheduleUpdateModel, Schedule>();
        }
    }
}