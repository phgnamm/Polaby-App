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
        private readonly IRatingRepository _ratingRepository;
        private readonly IEmotionRepository _emmotionRepository;
        private readonly INoteRepository _noteRepository;
        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository,
            IMenuRepository menuRepository, IMenuMealRepository menuMealRepository,
            ICommunityPostRepository communityPostRepository, IMealRepository mealRepository,
            IMealDishRepository mealDishRepository, IDishIngredientRepository dishIngredientRepository,
            IDishRepository dishRepository, IIngredientRepository ingredientRepository, INutrientRepository nutrientRepository,
            ICommentRepostiory commentRepostiory, IFollowRepository followRepository, IScheduleRepository scheduleRepository, IReportRepository reportRepository, IRatingRepository ratingRepository, IEmotionRepository emotionRepository,INoteRepository noteRepository
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
            _commentRepository = commentRepostiory;
            _followRepository = followRepository;
            _scheduleRepository = scheduleRepository;
            _reportRepository = reportRepository;
            _ratingRepository = ratingRepository;
            _emmotionRepository = emotionRepository;
            _noteRepository = noteRepository;
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
        public IRatingRepository RatingRepository => _ratingRepository;
        public IEmotionRepository EmotionRepository => _emmotionRepository;
        public INoteRepository NoteRepository => _noteRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
