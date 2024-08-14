

using Polaby.Repositories.Models.SafeFoodModels;
using Polaby.Services.Common;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Models.SafeFoodModels;

namespace Polaby.Services.Interfaces
{
    public interface ISafeFoodService
    {
        Task<ResponseModel> AddSafeFood(SafeFoodCreateModel createModel);
        Task<Pagination<SafeFoodModel>> GetAllSafeFoods(SafeFoodFilterModel filterModel);
        Task<ResponseModel> UpdateSafeFood(Guid id, SafeFoodCreateModel updateModel);
        Task<ResponseModel> DeleteSafeFood(Guid id);
    }
}
