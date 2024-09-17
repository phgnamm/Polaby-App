using Polaby.Repositories.Models.DishModels;
using Polaby.Services.Common;
using Polaby.Services.Models.DishModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IDishService 
    {
        Task<ResponseModel> AddRangeDish(List<DishImportModel> dishes);
        Task<ResponseModel> UpdateDish(Guid id, DishUpdateModel updateModel);
        Task<ResponseModel> DeleteDish(Guid id);
        Task<Pagination<DishModel>> GetAllDish(DishFilterModel dishFilterModel);
        Task<ResponseModel> AddDishIngredient(DishIngredientCreateModel model);
        Task<ResponseModel> DeleteDishIngredient(Guid dishId, Guid ingredientId);
        Task<ResponseDataModel<DishModel>> GetById(Guid id);
    }
}
