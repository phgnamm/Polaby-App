﻿using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.AccountModels;
using Polaby.Repositories.Models.ReportModels;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.CommonModels;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.MenuModels;
using Polaby.Repositories.Models.MenuModels;
using Polaby.Services.Models.MealModels;
using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Models.DishModels;
using Polaby.Repositories.Models.DishModels;
using Polaby.Repositories.Models.ExpertRegistrationModels;
using Polaby.Services.Models.IngredientModels;
using Polaby.Repositories.Models.IngredientModels;
using Polaby.Services.Models.NutrientModels;
using Polaby.Repositories.Models.NutrientModels;
using Polaby.Repositories.Models.WeeklyPostModels;
using Polaby.Services.Models.ExpertRegistrationModels;
using Polaby.Services.Models.ScheduleModels;
using Polaby.Services.Models.ReportModels;
using Polaby.Services.Models.WeeklyPostModels;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.NotificationTypeModels;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.CommunityPostLikeModels;
using Polaby.Repositories.Models.RatingModel;
using Polaby.Repositories.Models.EmotionModels;
using Polaby.Repositories.Models.NoteModels;
using Polaby.Services.Models.NoteModels;
using Polaby.Services.Models.HealthModels;
using Polaby.Repositories.Models.HealthModels;
using Polaby.Services.Models.SafeFoodModels;
using Polaby.Repositories.Models.SafeFoodModels;

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

            //WeeklyPost
            CreateMap<WeeklyPostModel, WeeklyPost>().ReverseMap();
            CreateMap<WeeklyPostCreateModel, WeeklyPost>();

            //ExpertRegistration
            CreateMap<ExpertRegistrationCreateModel, ExpertRegistration>();
            CreateMap<ExpertRegistrationModel, ExpertRegistration>().ReverseMap();

            //Menu
            CreateMap<MenuImportModel, Menu>().ReverseMap();
            CreateMap<MenuUpdateModel, Menu>().ForMember(dest => dest.Nutrients, opt => opt.Ignore());
            CreateMap<MenuModel, Menu>().ReverseMap();

            //MenuMeal
            CreateMap<MenuMealCreateModel, List<MenuMeal>>()
           .ConvertUsing(src => src.MealIds.Select(mealId => new MenuMeal
           {
               MenuId = src.MenuId,
               MealId = mealId
           }).ToList());

            //Meal
            CreateMap<MealImportModel, Meal>().ReverseMap();
            CreateMap<MealUpdateModel, Meal>();
            CreateMap<MealModel, Meal>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()))
                .ReverseMap();

            //Dish
            CreateMap<DishImportModel, Dish>().ReverseMap();
            CreateMap<DishUpdateModel, Dish>().ForMember(dest => dest.Nutrients, opt => opt.Ignore());
            CreateMap<DishModel, Dish>().ReverseMap();

            //DishIngredient
            CreateMap<DishIngredientCreateModel, DishIngredient>().ReverseMap();

            //Ingredient
            CreateMap<IngredientImportModel, Ingredient>().ReverseMap();
            CreateMap<IngredientUpdateModel, Ingredient>().ForMember(dest => dest.Nutrients, opt => opt.Ignore());
            CreateMap<IngredientModel, Ingredient>().ReverseMap();

            //Nutrient
            CreateMap<NutrientImportModel, Nutrient>().ReverseMap();
            CreateMap<NutrientUpdateModel, Nutrient>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<NutrientModel, Nutrient>().ReverseMap();

            //Report
            CreateMap<ReportCreateModel, Report>();
            CreateMap<ReportModel, Report>().ReverseMap();

            //CommunityPost
            CreateMap<CommunityPostCreateModel, CommunityPost>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId));
            CreateMap<CommunityPost, CommunityPostModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AccountId));
            CreateMap<CommunityPostUpdateModel, CommunityPost>();

            //Comment
            CreateMap<CommentCreateModel, Comment>();
            CreateMap<Comment, CommentModel>();
            CreateMap<CommentUpdateModel, Comment>();

            //Schedule
            CreateMap<ScheduleCreateModel, Schedule>();
            CreateMap<Schedule, ScheduleModel>();
            CreateMap<ScheduleUpdateModel, Schedule>();

            //NotificationSetting
            CreateMap<NotificationSettingUpdateModel, NotificationSetting>();
            CreateMap<NotificationSetting, NotificationSettingModel>();

            //NotificationType
            CreateMap<NotificationTypeModel, NotificationType>();
            CreateMap<NotificationType, NotificationTypeModel>();

            //CommentLike
            CreateMap<CommentLikeModel, CommentLike>();
            CreateMap<CommentLike, CommentLikeModel>();

            //CommunityPostLike
            CreateMap<CommunityPostLikeModel, CommunityPostLike>();
            CreateMap<CommunityPostLike, CommunityPostLikeModel>();

            //Rating
            CreateMap<RatingModel, Rating>().ReverseMap();

            //Emotion
            CreateMap<EmotionModel, Emotion>().ReverseMap();

            //Note
            CreateMap<NoteModel, Note>().ReverseMap();
            CreateMap<Note, NoteRequestModel>().ReverseMap();


            //Health
            CreateMap<HealthCreateModel, Health>();
            CreateMap<HealthUpdateModel, Health>();
            CreateMap<Health, HealthModel>().ReverseMap();

            CreateMap<SafeFoodCreateModel, SafeFood>();
            CreateMap<SafeFoodModel, SafeFood>().ReverseMap();
        }
    }
}