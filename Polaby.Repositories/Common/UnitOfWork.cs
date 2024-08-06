using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuMealRepository _menuMealRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IMenuRepository menuRepository, IMenuMealRepository menuMealRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_menuRepository = menuRepository;
			_menuMealRepository = menuMealRepository;

        }

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IMenuRepository MenuRepository => _menuRepository;
        public IMenuMealRepository MenuMealRepository => _menuMealRepository;

        public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}
