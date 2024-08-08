using Polaby.Repositories.Models.ReportModels;
using Polaby.Services.Common;
using Polaby.Services.Models.ReportModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces;

public interface IReportService
{
    Task<ResponseModel> CreateReport(ReportCreateModel reportCreateModel);
    Task<ResponseDataModel<ReportModel>> GetReport(Guid id);
    Task<Pagination<ReportModel>> GetAllReports(ReportFilterModel reportFilterModel);
    Task<ResponseModel> UpdateReport(Guid id, ReportUpdateModel reportUpdateModel);
}