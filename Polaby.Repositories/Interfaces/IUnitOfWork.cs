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
        ICommentLikeRepository CommentLikeRepository { get; }
        ICommunityPostLikeRepository CommunityPostLikeRepository { get; }
        IRatingRepository RatingRepository { get; }	
		IEmotionRepository EmotionRepository { get; }
		INoteRepository NoteRepository { get; }
        IHealthRepository HealthRepository { get; }
        ISafeFoodRepository SafeFoodRepository { get; }
        IIngredientSearchRepository IngredientSearchRepository { get; }
        IIngredientSearchNutrientRepository IngredientSearchNutrientRepository { get; }
        ISubscriptionFormRepository SubscriptionFormRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
