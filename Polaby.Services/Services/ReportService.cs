using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.ReportModels;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ReportModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> CreateReport(ReportCreateModel reportCreateModel)
    {
        var userId = _claimsService.GetCurrentUserId;

        if (reportCreateModel.CommentId == null && reportCreateModel.CommunityPostId == null && userId == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Comment id or community post id is needed"
            };
        }

        var existedReport = await _unitOfWork.ReportRepository.GetReportByUserAndResourceId(userId,
            reportCreateModel.CommentId, reportCreateModel.CommunityPostId);

        if (existedReport != null)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "User has already reported this item"
            };
        }

        if (reportCreateModel.CommentId != null && reportCreateModel.CommunityPostId == null)
        {
            var comment = await _unitOfWork.CommentRepository.GetAsync(reportCreateModel.CommentId.Value);

            if (comment == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Comment not found"
                };
            }
        }

        if (reportCreateModel.CommentId == null && reportCreateModel.CommunityPostId != null)
        {
            var comment = await _unitOfWork.CommunityPostRepository.GetAsync(reportCreateModel.CommunityPostId.Value);

            if (comment == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Community post not found"
                };
            }
        }

        var report = _mapper.Map<Report>(reportCreateModel);
        report.Status = ReportStatus.Pending;
        await _unitOfWork.ReportRepository.AddAsync(report);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Report this item successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot report this item"
        };
    }

    public async Task<ResponseDataModel<ReportModel>> GetReport(Guid id)
    {
        var report = await _unitOfWork.ReportRepository.GetAsync(id);

        if (report == null)
        {
            return new ResponseDataModel<ReportModel>
            {
                Status = false,
                Message = "Report not found"
            };
        }

        var reportModel = _mapper.Map<ReportModel>(report);

        return new ResponseDataModel<ReportModel>
        {
            Status = true,
            Message = "Get report successfully",
            Data = reportModel
        };
    }

    public async Task<Pagination<ReportModel>> GetAllReports(ReportFilterModel reportFilterModel)
    {
        var reportList = await _unitOfWork.ReportRepository.GetAllAsync(pageIndex: reportFilterModel.PageIndex,
            pageSize: reportFilterModel.PageSize,
            include: "Comment, CommunityPost",
            // filter: (x =>
            //     x.IsDeleted == reportFilterModel.IsDeleted &&
            //     (reportFilterModel.Status == null || x.Status == reportFilterModel.Status) &&
            //     (reportFilterModel.Reason == null || x.Reason == reportFilterModel.Reason)  &&
            //     (string.IsNullOrEmpty(reportFilterModel.Search) ||
            //      (x.Note == null || x.Note.ToLower().Contains(reportFilterModel.Search.ToLower()))
            //      ||
            //      (x.Comment == null || x.Comment.IsDeleted == false &&
            //          x.Comment.Content!.ToLower().Contains(reportFilterModel.Search.ToLower()))
            //      ||
            //      (x.CommunityPost == null || x.CommunityPost.IsDeleted == false &&
            //          x.CommunityPost.Content!.ToLower().Contains(reportFilterModel.Search.ToLower())))),
            filter: x =>
                x.IsDeleted == reportFilterModel.IsDeleted &&
                (reportFilterModel.Status == null || x.Status == reportFilterModel.Status) &&
                (reportFilterModel.Reason == null || x.Reason == reportFilterModel.Reason) &&
                (reportFilterModel.CommentId == null && x.Comment != null && !x.Comment.IsDeleted ||
                 x.CommentId == reportFilterModel.CommentId) &&
                (reportFilterModel.CommunityPostId == null && x.CommunityPost != null && !x.CommunityPost.IsDeleted ||
                 x.CommunityPostId == reportFilterModel.CommunityPostId) &&
                (string.IsNullOrEmpty(reportFilterModel.Search) ||
                 (x.Note != null &&
                  x.Note.IndexOf(reportFilterModel.Search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (x.Comment != null && !x.Comment.IsDeleted &&
                  x.Comment.Content != null &&
                  x.Comment.Content.IndexOf(reportFilterModel.Search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (x.CommunityPost != null && !x.CommunityPost.IsDeleted &&
                  x.CommunityPost.Content != null &&
                  (x.CommunityPost.Content.IndexOf(reportFilterModel.Search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   x.CommunityPost.Title.IndexOf(reportFilterModel.Search, StringComparison.OrdinalIgnoreCase) >= 0))),
            orderBy:
            (x =>
            {
                switch (reportFilterModel.Order.ToLower())
                {
                    case "reason":
                        return reportFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Reason)
                            : x.OrderBy(x => x.Reason);
                    case "status":
                        return reportFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Status)
                            : x.OrderBy(x => x.Status);
                    default:
                        return reportFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            })
        );

        var reportModelList = _mapper.Map<List<ReportModel>>(reportList.Data);
        return new Pagination<ReportModel>(reportModelList, reportList.TotalCount,
            reportFilterModel.PageIndex,
            reportFilterModel.PageSize);
    }

    public async Task<ResponseModel> UpdateReport(Guid id, ReportUpdateModel reportUpdateModel)
    {
        var report = await _unitOfWork.ReportRepository.GetAsync(id);

        if (report == null)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Report not found"
            };
        }

        report.Status = reportUpdateModel.Status;
        _unitOfWork.ReportRepository.Update(report);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Update report successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot update report",
        };
    }

    public async Task<ResponseModel> DeleteReport(Guid id)
    {
        var report = await _unitOfWork.ReportRepository.GetAsync(id);

        if (report == null)
        {
            return new ResponseDataModel<ReportModel>
            {
                Status = false,
                Message = "Report not found"
            };
        }

        _unitOfWork.ReportRepository.SoftDelete(report);

        if (await _unitOfWork.SaveChangeAsync() > 0)
        {
            return new ResponseModel
            {
                Status = true,
                Message = "Delete report successfully"
            };
        }

        return new ResponseModel
        {
            Status = false,
            Message = "Cannot delete report",
        };
    }
}