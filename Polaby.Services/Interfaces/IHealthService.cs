using Polaby.Repositories.Models.HealthModels;
using Polaby.Services.Common;
using Polaby.Services.Models.HealthModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IHealthService
    {
        Task<ResponseModel> AddHealthAsync(List<HealthCreateModel> healthModels);
        Task<Pagination<HealthModel>> GetAllHealthAsync(HealthFilterModel filterModel);
        Task<ResponseModel> UpdateHealthAsync(Guid id, HealthUpdateModel updateModel);
        Task<ResponseModel> DeleteHealthAsync(Guid id);
        Task<ResponseDataModel<HealthModel>> GetById(Guid id);
    }
}
