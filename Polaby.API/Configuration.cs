using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Common;
using System.Diagnostics;
using Polaby.Repositories.Entities;
using Polaby.Repositories;
using Polaby.Services.Interfaces;
using Polaby.Services.Services;
using Microsoft.AspNetCore.Identity;
using Polaby.API.Middlewares;
using Polaby.API.Utils;
using Polaby.Repositories.Repositories;
using Polaby.Services.Common;
using Polaby.API.Helper;
using Polaby.Services.Notification;

namespace Polaby.API
{
    public static class Configuration
    {
        public static IServiceCollection AddAPIConfiguration(this IServiceCollection services)
        {
            // Identity
            services
                .AddIdentity<Account, Role>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15);
            });

            // Middlewares
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();
            services.AddScoped<AccountStatusMiddleware>();
            services.AddSingleton<Stopwatch>();

            // Common
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();
           
            // Dependency Injection
            // Account
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Dashboard
            services.AddScoped<IDashboardService, DashboardService>();
            
            // Transaction
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            
            // WeeklyPost
            services.AddScoped<IWeeklyPostService, WeeklyPostService>();
            services.AddScoped<IWeeklyPostRepository, WeeklyPostRepository>();
            
            // SubscriptionForm
            services.AddScoped<ISubscriptionFormService, SubscriptionFormService>();
            services.AddScoped<ISubscriptionFormRepository, SubscriptionFormRepository>();

            // ExpertRegistration
            services.AddScoped<IExpertRegistrationService, ExpertRegistrationService>();
            services.AddScoped<IExpertRegistrationRepository, ExpertRegistrationRepository>();

            //Menu
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            //MenuMeal
            services.AddScoped<IMenuMealRepository, MenuMealRepository>();

            //Meal
            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IMealRepository, MealRepository>();

            //MealDish
            services.AddScoped<IMealDishRepository, MealDishRepository>();

            //Dish
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IDishRepository, DishRepository>();

            //DishIngredient
            services.AddScoped<IDishIngredientRepository, DishIngredientRepository>();

            //Ingredient
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();

            //Nutrient
            services.AddScoped<INutrientRepository, NutrientRepository>();

            //CommunityPost
            services.AddScoped<ICommunityPostService, CommunityPostService>();
            services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();

            //Comment
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICommentRepostiory, CommentRepository>();

            //Follow
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<IFollowRepository, FollowRepository>();

            //Schedule
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();

            //Report
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();

            //UserMenu
            services.AddScoped<IUserMenuRepository, UserMenuRepository>();

            //Notification
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            //NotificationSetting
            services.AddScoped<INotificationSettingService, NotificationSettingService>();
            services.AddScoped<INotificationSettingRepository, NotificationSettingRepository>();

            //NotificationType
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();

            //CommentLike
            services.AddScoped<ICommentLikeService, CommentLikeService>();
            services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();

            //CommunityPostLike
            services.AddScoped<ICommunityPostLikeService, CommunityPostLikeService>();
            services.AddScoped<ICommunityPostLikeRepository, CommunityPostLikeRepository>();

            //Rating
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRatingRepository,RatingRepository>();

            //Emotion
            services.AddScoped<IEmotionRepository, EmotionRepository>();
            services.AddScoped<IEmotionService, EmotionService>();

            //Note
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<INoteRepository, NoteRepository>();
            //Health
            services.AddScoped<IHealthService, HealthService>();
            services.AddScoped<IHealthRepository, HealthRepository>();

            //SafeFood
            services.AddScoped<ISafeFoodRepository, SafeFoodRepository>();
            services.AddScoped<ISafeFoodService, SafeFoodService>();

            //BackgroudService
            services.AddHostedService<SubscriptionBackgroundService>();

            //IngredientSearch
            services.AddScoped<IIngredientSearchService, IngredientSearchService>();
            services.AddScoped<IIngredientSearchRepository, IngredientSearchRepository>();

            //IngredientSearchNutrient
            services.AddScoped<IIngredientSearchNutrientRepository, IngredientSearchNutrientRepository>();

            services.AddScoped<IOneSignalPushNotificationService, OneSignalPushNotificationService>();
            services.AddHttpClient<OneSignalPushNotificationService>();

            return services;
        }
    }
}