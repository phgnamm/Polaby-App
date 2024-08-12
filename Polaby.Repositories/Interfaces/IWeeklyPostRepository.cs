using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Interfaces;

public interface IWeeklyPostRepository : IGenericRepository<WeeklyPost>
{
    Task<WeeklyPost?> GetPostByWeek(int week);
    Task<List<int>?> GetValidWeeks(List<int> weeks);
}