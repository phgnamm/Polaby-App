using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepostiory
    {
        private readonly AppDbContext _dbContext;

        public CommentRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> GetById(Guid id)
        {
            return await _dbContext.Comment
                .Include(cp => cp.Account)
                .Include(cp => cp.Reports)
                .Include(cp => cp.CommentLikes)
                .Include(cp => cp.CommentReplies)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
    }
}