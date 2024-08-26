using Polaby.Repositories.Interfaces;
using System.Security.AccessControl;

namespace Polaby.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommunityPostRepository _communityPostRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuMealRepository _menuMealRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealDishRepository _mealDishRepository;
        private readonly IDishIngredientRepository _dishIngredientRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly INutrientRepository _nutrientRepository;
        private readonly ICommentRepostiory _commentRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IWeeklyPostRepository _weeklyPostRepository;
        private readonly IExpertRegistrationRepository _expertRegistrationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationSettingRepository _notificationSettingRepository;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IUserMenuRepository _userMenuRepository;
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly ICommunityPostLikeRepository _communityPostLikeRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IEmotionRepository _motionRepository;
        private readonly INoteRepository _noteRepository;
        private readonly IHealthRepository _healthRepository;
        private readonly ISafeFoodRepository _safeFoodRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository,
            IMenuRepository menuRepository, IMenuMealRepository menuMealRepository,
            ICommunityPostRepository communityPostRepository, IMealRepository mealRepository,
            IMealDishRepository mealDishRepository, IDishIngredientRepository dishIngredientRepository,
            IDishRepository dishRepository, IIngredientRepository ingredientRepository,
            INutrientRepository nutrientRepository,
            ICommentRepostiory commentRepository, IFollowRepository followRepository,
            IScheduleRepository scheduleRepository, IReportRepository reportRepository,
            IWeeklyPostRepository weeklyPostRepository, IExpertRegistrationRepository expertRegistrationRepository,
            INotificationRepository notificationRepository,
            INotificationTypeRepository notificationTypeRepository,
            INotificationSettingRepository notificationSettingRepository,
            IUserMenuRepository userMenuRepository,
            ICommentLikeRepository commentLikeRepository,
            ICommunityPostLikeRepository communityPostLikeRepository,
            IRatingRepository ratingRepository,
            IEmotionRepository motionRepository,
            INoteRepository noteRepository,
            IHealthRepository healthRepository,
            ISafeFoodRepository safeFoodRepository
        )
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _menuRepository = menuRepository;
            _menuMealRepository = menuMealRepository;
            _communityPostRepository = communityPostRepository;
            _mealRepository = mealRepository;
            _mealDishRepository = mealDishRepository;
            _dishRepository = dishRepository;
            _dishIngredientRepository = dishIngredientRepository;
            _ingredientRepository = ingredientRepository;
            _nutrientRepository = nutrientRepository;
            _commentRepository = commentRepository;
            _followRepository = followRepository;
            _scheduleRepository = scheduleRepository;
            _reportRepository = reportRepository;
            _weeklyPostRepository = weeklyPostRepository;
            _expertRegistrationRepository = expertRegistrationRepository;
            _notificationRepository = notificationRepository;
            _notificationTypeRepository = notificationTypeRepository;
            _notificationSettingRepository = notificationSettingRepository;
            _userMenuRepository = userMenuRepository;
            _commentLikeRepository = commentLikeRepository;
            _communityPostLikeRepository = communityPostLikeRepository;
            _ratingRepository = ratingRepository;
            _motionRepository = motionRepository;
            _noteRepository = noteRepository;
            _healthRepository = healthRepository;
            _safeFoodRepository = safeFoodRepository;
        }

        public AppDbContext DbContext => _dbContext;
        public IAccountRepository AccountRepository => _accountRepository;
        public ICommunityPostRepository CommunityPostRepository => _communityPostRepository;
        public IMenuRepository MenuRepository => _menuRepository;
        public IMenuMealRepository MenuMealRepository => _menuMealRepository;
        public IMealRepository MealRepository => _mealRepository;
        public IMealDishRepository MealDishRepository => _mealDishRepository;
        public IDishRepository DishRepository => _dishRepository;
        public IDishIngredientRepository DishIngredientRepository => _dishIngredientRepository;
        public IIngredientRepository IngredientRepository => _ingredientRepository;
        public INutrientRepository NutrientRepository => _nutrientRepository;
        public ICommentRepostiory CommentRepository => _commentRepository;
        public IFollowRepository FollowRepository => _followRepository;
        public IScheduleRepository ScheduleRepository => _scheduleRepository;
        public IReportRepository ReportRepository => _reportRepository;
        public IWeeklyPostRepository WeeklyPostRepository => _weeklyPostRepository;
        public IExpertRegistrationRepository ExpertRegistrationRepository => _expertRegistrationRepository;
        public INotificationRepository NotificationRepository => _notificationRepository;
        public INotificationSettingRepository NotificationSettingRepository => _notificationSettingRepository;
        public INotificationTypeRepository NotificationTypeRepository => _notificationTypeRepository;
        public IUserMenuRepository UserMenuRepository => _userMenuRepository;
        public ICommentLikeRepository CommentLikeRepository => _commentLikeRepository;
        public ICommunityPostLikeRepository CommunityPostLikeRepository => _communityPostLikeRepository;
        public IHealthRepository HealthRepository => _healthRepository;
        public ISafeFoodRepository SafeFoodRepository => _safeFoodRepository;

        public IRatingRepository RatingRepository => _ratingRepository;

        public IEmotionRepository EmotionRepository => _motionRepository;

        public INoteRepository NoteRepository => _noteRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
