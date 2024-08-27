using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.QueryModels;
using System.Linq.Expressions;

namespace Polaby.Repositories.Interfaces
{
    public interface ICommunityPostRepository : IGenericRepository<CommunityPost>
    {
        Task<QueryResultModel<List<CommunityPost>>> GetAllPostWithFollowingCondition(
     Expression<Func<CommunityPost, bool>>? filter = null,
     Func<IQueryable<CommunityPost>, IOrderedQueryable<CommunityPost>>? orderBy = null,
     string include = "",
     int? pageIndex = null,
     int? pageSize = null,
     Guid? currentUserId = null,
     bool? isFollowing = null);
        Task<CommunityPost> GetById(Guid id);
    }
}
