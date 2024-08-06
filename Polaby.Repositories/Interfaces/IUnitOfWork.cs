namespace Polaby.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		AppDbContext DbContext { get; }
		IAccountRepository AccountRepository { get; }
        ICommuntityPostRepository CommuntityPostRepository { get; }

        public Task<int> SaveChangeAsync();
	}
}
