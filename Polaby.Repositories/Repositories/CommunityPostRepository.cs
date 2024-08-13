using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Common;
using Polaby.Repositories.Models.QueryModels;

namespace Polaby.Repositories.Repositories
{
    public class CommunityPostRepository : GenericRepository<CommunityPost>, ICommunityPostRepository
    {
        private readonly AppDbContext _dbContext;

        public CommunityPostRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<QueryResultModel<List<CommunityPost>>> GetAllPostWithFollowingCondition(
             Expression<Func<CommunityPost, bool>>? filter = null,
             Func<IQueryable<CommunityPost>, IOrderedQueryable<CommunityPost>>? orderBy = null,
             string include = "",
             int? pageIndex = null,
             int? pageSize = null,
             Guid? currentUserId = null,
             bool? isFollowing = null)
        {
            IQueryable<CommunityPost> query = _dbSet;

            if ((bool)isFollowing)
            {
                query = query
                    .Join(_dbContext.Follow.Where(f => f.UserId == currentUserId),
                          post => post.AccountId,
                          follow => follow.ExpertId,
                          (post, follow) => post);
            }
            else if (filter != null && !(bool)isFollowing)
            {
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();

            foreach (var includeProperty in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<CommunityPost>>()
            {
                TotalCount = totalCount,
                Data = await query.ToListAsync(),
            };
        }

    }
}
