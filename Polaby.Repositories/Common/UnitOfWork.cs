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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommuntityPostRepository _communtityPostRepository;
        private readonly ICommentRepostiory _commentRepostiory;
        private readonly IFollowRepository _followRepository;
        private readonly IRatingRepository _ratingRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, ICommuntityPostRepository communtityPostRepository,
            ICommentRepostiory commentRepostiory, IFollowRepository followRepository, IRatingRepository ratingRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _communtityPostRepository = communtityPostRepository;
            _commentRepostiory = commentRepostiory;
            _followRepository = followRepository;
            _ratingRepository = ratingRepository;
        }

        public AppDbContext DbContext => _dbContext;
        public IAccountRepository AccountRepository => _accountRepository;
        public ICommuntityPostRepository CommuntityPostRepository => _communtityPostRepository;
        public ICommentRepostiory CommentRepostiory => _commentRepostiory;
        public IFollowRepository FollowRepository => _followRepository;
        public IScheduleRepository ScheduleRepository => _scheduleRepository;
        public IReportRepository ReportRepository => _reportRepository;

        public IRatingRepository RatingRepository => _ratingRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
