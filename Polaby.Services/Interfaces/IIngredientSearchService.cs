
using Polaby.Repositories.Models.IngredientSearchModels;
using Polaby.Services.Common;
using Polaby.Services.Models.IngredientSearchModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IIngredientSearchService
    {
        Task<ResponseModel> AddIngredientSearchWithNutrients(IngredientSearchtImportModel ingredientImport);
        Task<Pagination<IngredientSearchModel>> GetAllIngredient(IngredientSearchFilterModel ingredientFilterModel);
        Task<ResponseModel> UpdateIngredient(Guid id, IngredientSearchUpdateModel updateModel);
        Task<ResponseModel> DeleteIngredient(Guid id);
        Task<ResponseDataModel<IngredientSearchModel>> GetById(Guid id);
    }
}
