using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.DashboardModels;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.DashboardModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDataModel<AdminDashboardModel>> GetAdminDashboard(
        DashboardFilterModel dashboardFilterModel)
    {
        // Create a date range for the specified month and year
        var startDate = new DateTime(dashboardFilterModel.Year, dashboardFilterModel.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

        int totalAccount = await _unitOfWork.DbContext.Users
            .Where(x => !x.IsDeleted)
            .CountAsync();

        int totalSubscription = await _unitOfWork.DbContext.Users
            .Where(x => x.IsSubscriptionActive && !x.IsDeleted)
            .CountAsync();

        int totalPost = await _unitOfWork.DbContext.CommunityPost
            .Where(x => !x.IsDeleted && x.CreationDate >= startDate && x.CreationDate <= endDate)
            .CountAsync();

        float totalEarning = await _unitOfWork.DbContext.Transaction
            .Where(x => !x.IsDeleted && x.Status == TransactionStatus.Completed && x.CreationDate >= startDate &&
                        x.CreationDate <= endDate)
            .SumAsync(x => x.Amount);

        // Get the sum of transactions for each day within the specified month and year
        var earningsGroupedByDate = await _unitOfWork.DbContext.Transaction
            .Where(x => !x.IsDeleted && x.Status == TransactionStatus.Completed && x.CreationDate >= startDate &&
                        x.CreationDate <= endDate)
            .GroupBy(x => x.CreationDate.Date)
            .Select(g => new
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalAmount = g.Sum(x => x.Amount)
            })
            .ToListAsync();

        // Create a list of all days in the month
        var allDaysInMonth = Enumerable.Range(0,
                DateTime.DaysInMonth(dashboardFilterModel.Year, dashboardFilterModel.Month))
            .Select(day => new DateOnly(dashboardFilterModel.Year, dashboardFilterModel.Month, day + 1))
            .ToList();

        // Create a list of Earning objects, ensuring all days are included
        List<Earning> earnings = allDaysInMonth
            .Select(day => new Earning
            {
                Date = day,
                Amount = earningsGroupedByDate.FirstOrDefault(e => e.Date == day)?.TotalAmount ?? 0
            })
            .ToList();

        // Get the number of posts grouped by date within the specified month and year
        var forumsGroupedByDate = await _unitOfWork.DbContext.CommunityPost
            .Where(x => !x.IsDeleted && x.CreationDate >= startDate && x.CreationDate <= endDate)
            .GroupBy(x => x.CreationDate.Date)
            .Select(g => new
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalPost = g.Count()
            })
            .ToListAsync();

        // Create a list of Forum objects, ensuring all days are included
        List<Forum> forums = allDaysInMonth
            .Select(day => new Forum
            {
                Date = day,
                TotalPost = forumsGroupedByDate.FirstOrDefault(f => f.Date == day)?.TotalPost ?? 0
            })
            .ToList();

        // Return the assembled dashboard data
        return new ResponseDataModel<AdminDashboardModel>
        {
            Message = "Get admin dashboard successfully",
            Status = true,
            Data = new AdminDashboardModel
            {
                TotalAccount = totalAccount,
                TotalSubscription = totalSubscription,
                TotalPost = totalPost,
                TotalEarning = totalEarning,
                Earnings = earnings,
                Forums = forums
            }
        };
    }
}