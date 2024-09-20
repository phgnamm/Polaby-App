using Polaby.Repositories.Models.DashboardModels;
using Polaby.Services.Models.DashboardModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces;

public interface IDashboardService
{
    Task<ResponseDataModel<AdminDashboardModel>> GetAdminDashboard(DashboardFilterModel dashboardFilterModel);
}