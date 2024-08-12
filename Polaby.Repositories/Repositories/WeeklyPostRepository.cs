using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;

namespace Polaby.Repositories.Repositories;

public class WeeklyPostRepository : GenericRepository<WeeklyPost>, IWeeklyPostRepository
{
    public WeeklyPostRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
    }

    public async Task<WeeklyPost?> GetPostByWeek(int week)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Week == week);
    }

    public async Task<List<int>?> GetValidWeeks(List<int> weeks)
    {
        var query = weeks.AsQueryable();
        var existingWeeks = _dbSet.Select(x => x.Week!.Value);
        var result = query.Where(x => !existingWeeks.Any(w => w == x));
        List<int> list = await result.ToListAsync();
        return list;
    }
}