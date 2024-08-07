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

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository,
            IMenuRepository menuRepository, IMenuMealRepository menuMealRepository,
            ICommuntityPostRepository communtityPostRepository, IMealRepository mealRepository,
            IMealDishRepository mealDishRepository, IDishIngredientRepository dishIngredientRepository,
            IDishRepository dishRepository
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

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
