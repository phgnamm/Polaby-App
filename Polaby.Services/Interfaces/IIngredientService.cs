

using Polaby.Repositories.Models.IngredientModels;
using Polaby.Repositories.Models.MealModels;
using Polaby.Services.Common;
using Polaby.Services.Models.IngredientModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<ResponseModel> AddRangeIngredient(List<IngredientImportModel> ingredients);
        Task<ResponseModel> UpdateIngredient(Guid id, IngredientUpdateModel updateModel);
        Task<ResponseModel> DeleteIngredient(Guid id);
        Task<Pagination<IngredientModel>> GetAllIngredient(IngredientFilterModel ingredientFilterModel);
    }
}
