using Polaby.Repositories.Models.EmotionModels;
using Polaby.Services.Models.EmotionModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Interfaces
{
    public interface IEmotionService
    {
        Task<ResponseDataModel<EmotionModel>> AddEmotionAsync(EmotionRequestModel model);
        Task<ResponseDataModel<EmotionModel>> DeleteEmotionAsync(Guid id);
    }
}
