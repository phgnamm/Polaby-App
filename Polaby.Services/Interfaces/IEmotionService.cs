﻿using Polaby.Repositories.Models.EmotionModels;
using Polaby.Services.Models.EmotionModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Interfaces
{
    public interface IEmotionService
    {
        Task<ResponseDataModel<EmotionModel>> AddEmotionAsync(EmotionRequestModel model);
        Task<ResponseDataModel<EmotionModel>> DeleteEmotionAsync(Guid id);
    }
}
