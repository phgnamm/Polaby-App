using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
        private readonly ICommuntityPostRepository _communtityPostRepository;
        private readonly ICommentRepostiory _commentRepostiory;
        private readonly IFollowRepository _followRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, ICommuntityPostRepository communtityPostRepository,
			ICommentRepostiory commentRepostiory, IFollowRepository followRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_communtityPostRepository = communtityPostRepository;
			_commentRepostiory = commentRepostiory;
			_followRepository = followRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
        public ICommuntityPostRepository CommuntityPostRepository => _communtityPostRepository;
        public ICommentRepostiory CommentRepostiory => _commentRepostiory;
        public IFollowRepository FollowRepository => _followRepository;

        public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}
