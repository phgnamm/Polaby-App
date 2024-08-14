using Polaby.Repositories.Entities;
using Polaby.Repositories.Models.MenuModels;
using Polaby.Repositories.Models.RatingModel;
using Polaby.Services.Common;
using Polaby.Services.Models.MenuModels;
using Polaby.Services.Models.RatingModel;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Interfaces
{
    public interface IRatingService
    {
        Task<ResponseDataModel<Rating>> CreateRatingAsync(CreateRatingModel model);
        Task<ResponseDataModel<Rating?>> UpdateRatingAsync(CreateRatingModel model);
        Task<ResponseModel> DeleteRatingAsync(Guid userId, Guid expertId);
        Task<Pagination<RatingModel>> GetRatingsByFilterAsync(RatingFilterModel model);


    }
}
