
using Polaby.Services.Common;
using Polaby.Repositories.Models.MenuModels;
using Polaby.Services.Models.MenuModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Repositories.Entities;

namespace Polaby.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ResponseModel> AddRangeMenu(List<MenuImportModel> menus);
        Task<ResponseModel> UpdateMenu(Guid id, MenuUpdateModel updateModel);
        Task<ResponseModel> DeleteMenu(Guid id);
        Task<Pagination<MenuModel>> GetAllMenu(MenuFilterModel menuFilterModel);
        Task<ResponseModel> AddRangeMenuMeal(List<MenuMealCreateModel> menuMeals);
        Task<Pagination<MenuModel>> GetMenuRecommendations(Guid accountId);
    }
}
