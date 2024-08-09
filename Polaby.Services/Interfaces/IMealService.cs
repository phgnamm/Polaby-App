using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Common;
using Polaby.Services.Models.MealModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IMealService
    {
        Task<ResponseModel> AddRangeMeal(List<MealImportModel> meals);
        Task<ResponseModel> UpdateMeal(Guid id, MealUpdateModel updateModel);
        Task<ResponseModel> DeleteMeal(Guid id);
        Task<Pagination<MealModel>> GetAllMeal(MealFilterModel mealFilterModel);
    }
}
