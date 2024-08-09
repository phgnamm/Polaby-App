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

            return services;
        }
    }
}