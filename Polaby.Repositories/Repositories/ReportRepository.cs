using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
    }

    public async Task<Report?> GetReportByUserAndResourceId(Guid? userId, Guid? commentId = null,
        Guid? communityPostId = null)
    {
        return await _dbSet.FirstOrDefaultAsync(x =>
            x.CreatedBy == userId && x.CommentId == commentId && x.CommunityPostId == communityPostId &&
            x.IsDeleted == false);
    }
}