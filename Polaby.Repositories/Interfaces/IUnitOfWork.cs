namespace Polaby.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		AppDbContext DbContext { get; }
		IAccountRepository AccountRepository { get; }
		IMenuRepository MenuRepository { get; }
        IMenuMealRepository MenuMealRepository { get; }
        public Task<int> SaveChangeAsync();
	}
}
