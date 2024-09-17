using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class CommentLikeRepository : GenericRepository<CommentLike>, ICommentLikeRepository
    {
        private readonly AppDbContext _dbContext;

        public CommentLikeRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<CommentLike> GetByUserAndComment(Guid userId, Guid commentId)
        {
            return await _dbContext.CommentLike
                                   .FirstOrDefaultAsync(x => x.UserId == userId && x.CommentId == commentId);
        }
    }
}
