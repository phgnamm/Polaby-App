using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.RatingModel;
using Polaby.Services.Common;
using Polaby.Services.Models.RatingModel;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IRatingService
    {
        Task<ResponseDataModel<Rating>> CreateRatingAsync(CreateRatingModel model);
        Task<ResponseDataModel<Rating?>> UpdateRatingAsync(Guid id,CreateRatingModel model);
        Task<ResponseModel> DeleteRatingAsync(Guid id);
        Task<Pagination<RatingModel>> GetRatingsByFilterAsync(RatingFilterModel model);
        Task<ResponseDataModel<RatingModel>> GetById(Guid id);

    }
}
