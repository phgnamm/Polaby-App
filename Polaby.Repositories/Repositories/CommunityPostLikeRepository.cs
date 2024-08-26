using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class CommunityPostLikeRepository : GenericRepository<CommunityPostLike>, ICommunityPostLikeRepository
    {
        private readonly AppDbContext _dbContext;

        public CommunityPostLikeRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<CommunityPostLike> GetByUserAndPost(Guid userId, Guid postId)
        {
            return await _dbContext.CommunityPostLike
                                   .FirstOrDefaultAsync(x => x.UserId == userId && x.CommunityPostId == postId);
        }
    }
}
