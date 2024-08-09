namespace Polaby.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		AppDbContext DbContext { get; }
		IAccountRepository AccountRepository { get; }
        ICommuntityPostRepository CommuntityPostRepository { get; }
        ICommentRepostiory CommentRepostiory { get; }
        IFollowRepository FollowRepository { get; }
		IRatingRepository RatingRepository { get; }	

        public Task<int> SaveChangeAsync();
	}
}
