using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces
{
    public interface ICommentLikeRepository : IGenericRepository<CommentLike>
    {
        Task<CommentLike> GetByUserAndComment(Guid userId, Guid commentId);
    }
}
