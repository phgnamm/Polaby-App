using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<Report?> GetReportByUserAndResourceId(Guid? userId, Guid? commentId = null, Guid? communityPostId = null);
}