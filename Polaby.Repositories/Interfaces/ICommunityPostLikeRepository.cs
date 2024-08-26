using Polaby.Repositories.Entities;
namespace Polaby.Repositories.Interfaces
{
    public interface ICommunityPostLikeRepository : IGenericRepository<CommunityPostLike>
    {
        Task<CommunityPostLike> GetByUserAndPost(Guid userId, Guid postId);
    }
}
