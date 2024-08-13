namespace Polaby.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        ICommunityPostRepository CommunityPostRepository { get; }
        IMenuRepository MenuRepository { get; }
        IMenuMealRepository MenuMealRepository { get; }
        IMealRepository MealRepository { get; }
        IMealDishRepository MealDishRepository { get; }
        IDishRepository DishRepository { get; }
        IDishIngredientRepository DishIngredientRepository { get; }
        IIngredientRepository IngredientRepository { get; }
        INutrientRepository NutrientRepository { get; }
        ICommentRepostiory CommentRepository { get; }
        IFollowRepository FollowRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IReportRepository ReportRepository { get; }
        IWeeklyPostRepository WeeklyPostRepository { get; }
        IExpertRegistrationRepository ExpertRegistrationRepository { get; }
        INotificationRepository NotificationRepository { get; }
        INotificationTypeRepository NotificationTypeRepository { get; }
        INotificationSettingRepository NotificationSettingRepository { get; }
        IUserMenuRepository UserMenuRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}