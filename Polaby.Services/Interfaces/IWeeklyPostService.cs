using Polaby.Repositories.Models.WeeklyPostModels;
using Polaby.Services.Common;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.WeeklyPostModels;

namespace Polaby.Services.Interfaces;

public interface IWeeklyPostService
{
    Task<ResponseModel> AddWeeklyPosts(List<WeeklyPostCreateModel> weeklyPostCreateModels);
    Task<ResponseDataModel<WeeklyPostModel>> GetWeeklyPost(int week);
    Task<ResponseDataModel<WeeklyPostModel>> GetWeeklyPost(Guid id);
    Task<Pagination<WeeklyPostModel>> GetAllWeeklyPosts(WeeklyPostFilterModel weeklyPostFilterModel);
    Task<ResponseModel> UpdateWeeklyPost(Guid id, WeeklyPostUpdateModel weeklyPostUpdateModel);
    Task<ResponseModel> DeleteWeeklyPosts(Guid id);
}