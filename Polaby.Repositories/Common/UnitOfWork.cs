using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommuntityPostRepository _communityPostRepository;
        private readonly ICommentRepostiory _commentRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IReportRepository _reportRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository,
            ICommuntityPostRepository communityPostRepository,
            ICommentRepostiory commentRepository, IFollowRepository followRepository,
            IReportRepository reportRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _communityPostRepository = communityPostRepository;
            _commentRepository = commentRepository;
            _followRepository = followRepository;
            _reportRepository = reportRepository;
        }

        public AppDbContext DbContext => _dbContext;
        public IAccountRepository AccountRepository => _accountRepository;
        public ICommuntityPostRepository CommunityPostRepository => _communityPostRepository;
        public ICommentRepostiory CommentRepository => _commentRepository;
        public IFollowRepository FollowRepository => _followRepository;
        public IReportRepository ReportRepository => _reportRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}