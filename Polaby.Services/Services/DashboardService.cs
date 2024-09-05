using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.DashboardModels;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDataModel<AdminDashboardModel>> GetAdminDashboard()
    {
        int totalAccount = await _unitOfWork.DbContext.Users
            .Where(x => !x.IsDeleted)
            .CountAsync();

        int totalSubscription = await _unitOfWork.DbContext.Users
            .Where(x => x.IsSubscriptionActive && !x.IsDeleted)
            .CountAsync();

        int totalPost = await _unitOfWork.DbContext.CommunityPost
            .Where(x => !x.IsDeleted)
            .CountAsync();

        float totalEarning = await _unitOfWork.DbContext.Transaction
            .Where(x => !x.IsDeleted && x.Status == TransactionStatus.Completed)
            .SumAsync(x => x.Amount);

        // // Get the first date from transactions
        // DateOnly firstDate = DateOnly.FromDateTime(await _unitOfWork.DbContext.Transaction
        //     .MinAsync(x => x.CreationDate));

        // Get the sum of transactions for each day
        var earningsGroupedByDate = await _unitOfWork.DbContext.Transaction
            .Where(x => !x.IsDeleted && x.Status == TransactionStatus.Completed)
            .GroupBy(x => x.CreationDate.Date) // Group by the date part of CreationDate
            .Select(g => new
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalAmount = g.Sum(x => x.Amount)
            })
            .ToListAsync();

        // Create a list of Earning objects from the grouped results
        List<Earning> earnings = earningsGroupedByDate
            .Select(e => new Earning
            {
                Date = e.Date,
                Amount = e.TotalAmount
            })
            .ToList();

        // Get the number of posts and comments grouped by date
        var forumsGroupedByDate = await _unitOfWork.DbContext.CommunityPost
            .Where(x => !x.IsDeleted)
            .GroupBy(x => x.CreationDate.Date) // Assuming `PostDate` is the date field for posts
            .Select(g => new
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalPost = g.Count(),
                // TotalComment =
                //     g.Sum(x => x.Comments.Count) // Assuming `Comments` is a collection of comments for each post
            })
            .ToListAsync();

        // Create a list of Forum objects from the grouped results
        List<Forum> forums = forumsGroupedByDate
            .Select(f => new Forum
            {
                Date = f.Date,
                TotalPost = f.TotalPost,
                // TotalComment = f.TotalComment
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