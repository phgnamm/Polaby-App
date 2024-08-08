using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommuntityPostRepository _communtityPostRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuMealRepository _menuMealRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealDishRepository _mealDishRepository;
        private readonly IDishIngredientRepository _dishIngredientRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly INutrientRepository _nutrientRepository;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommuntityPostRepository _communityPostRepository;
        private readonly ICommentRepostiory _commentRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IReportRepository _reportRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository,
            IMenuRepository menuRepository, IMenuMealRepository menuMealRepository,
            ICommuntityPostRepository communtityPostRepository, IMealRepository mealRepository,
            IMealDishRepository mealDishRepository, IDishIngredientRepository dishIngredientRepository,
            IDishRepository dishRepository, IIngredientRepository ingredientRepository, INutrientRepository nutrientRepository
            )
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _menuRepository = menuRepository;
            _menuMealRepository = menuMealRepository;
            _communtityPostRepository = communtityPostRepository;
            _mealRepository = mealRepository;
            _mealDishRepository = mealDishRepository;
            _dishRepository = dishRepository;
            _dishIngredientRepository = dishIngredientRepository;
            _ingredientRepository = ingredientRepository;
            _nutrientRepository = nutrientRepository;
        }
        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, ICommuntityPostRepository communtityPostRepository,
			ICommentRepostiory commentRepostiory, IFollowRepository followRepository, IScheduleRepository scheduleRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
            _communityPostRepository = communtityPostRepository;
            _commentRepository = commentRepostiory;
			_followRepository = followRepository;
			_scheduleRepository = scheduleRepository;
		}


        public AppDbContext DbContext => _dbContext;
        public IAccountRepository AccountRepository => _accountRepository;
        public ICommuntityPostRepository CommuntityPostRepository => _communtityPostRepository;
        public IMenuRepository MenuRepository => _menuRepository;
        public IMenuMealRepository MenuMealRepository => _menuMealRepository;
        public IMealRepository MealRepository => _mealRepository;
        public IMealDishRepository MealDishRepository => _mealDishRepository;
        public IDishRepository DishRepository => _dishRepository;
        public IDishIngredientRepository DishIngredientRepository => _dishIngredientRepository;
        public IIngredientRepository IngredientRepository => _ingredientRepository;
        public INutrientRepository NutrientRepository => _nutrientRepository;
        public AppDbContext DbContext => _dbContext;
        public IAccountRepository AccountRepository => _accountRepository;
        public ICommuntityPostRepository CommunityPostRepository => _communityPostRepository;
        public ICommentRepostiory CommentRepository => _commentRepository;
        public IFollowRepository FollowRepository => _followRepository;
        public IScheduleRepository ScheduleRepository => _scheduleRepository;
        public IReportRepository ReportRepository => _reportRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
