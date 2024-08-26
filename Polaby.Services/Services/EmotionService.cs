using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.EmotionModels;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.EmotionModels;
using Polaby.Services.Models.ResponseModels;
namespace Polaby.Services.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmotionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDataModel<EmotionModel>> AddEmotionAsync(EmotionRequestModel model)
        {
            var user = await _unitOfWork.AccountRepository.GetAccountById(model.UserId);
            if (user == null)
            {
                return new ResponseDataModel<EmotionModel>
                {
                    Status = false,
                    Message = "User does not exist.",
                    Data = null
                };
            }
            var emotion = new Emotion
            {
                UserId = model.UserId,
                Type = model.Type,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            await _unitOfWork.EmotionRepository.AddAsync(emotion);
            await _unitOfWork.SaveChangeAsync();

            var emotionModel = _mapper.Map<EmotionModel>(emotion);
            return new ResponseDataModel<EmotionModel>
            {
                Status = true,
                Message = "Emotion added successfully.",
                Data = emotionModel
            };
        }
        public async Task<ResponseDataModel<EmotionModel>> DeleteEmotionAsync(Guid emotionId)
        {
            // Tìm Emotion theo ID
            var emotion = await _unitOfWork.EmotionRepository.GetAsync(emotionId);
            if (emotion == null)
            {
                return new ResponseDataModel<EmotionModel>
                {
                    Status = false,
                    Message = "Emotion not found.",
                    Data = null
                };
            }

            // Xóa Emotion
            _unitOfWork.EmotionRepository.HardDelete(emotion);
            await _unitOfWork.SaveChangeAsync();

            var emotionModel = _mapper.Map<EmotionModel>(emotion);
            return new ResponseDataModel<EmotionModel>
            {
                Status = true,
                Message = "Emotion deleted successfully.",
                Data = emotionModel
            };
        }
    }
}
