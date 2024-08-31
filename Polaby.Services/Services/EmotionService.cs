using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.EmotionModels;
using Polaby.Services.Common;
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

        public async Task<ResponseDataModel<EmotionModel>> CreateEmotionAsync(EmotionRequestModel model)
        {
            var emotion = new Emotion
            {
                UserId = model.UserId,
                Date = model.Date,
                EmotionTypes = model.EmotionTypes.Select(et => new EmotionTypeMapping { Type = et }).ToList(),
                Notes = model.Notes
                     .Where(n => n.IsSelected)  // Chỉ thêm những note có IsSelected = true
                     .Select(n => new NoteEmotion
                     {
                         Content = n.Content,
                         IsSelected = n.IsSelected
                     }).ToList()
            };

            await _unitOfWork.EmotionRepository.AddAsync(emotion);
            await _unitOfWork.SaveChangeAsync();

            var emotionModel = _mapper.Map<EmotionModel>(emotion);

            return new ResponseDataModel<EmotionModel>
            {
                Status = true,
                Message = "Emotion created successfully.",
                Data = emotionModel
            };
        }

        public async Task<Pagination<EmotionModel>> GetEmotionsByFilterAsync(EmotionFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.EmotionRepository.GetAllAsync(
                filter: e => (filterModel.UserId == null || e.UserId == filterModel.UserId) && (filterModel.Date == null || e.Date == filterModel.Date),
                include: "EmotionTypes,Notes",  
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );
            var emotionModels = _mapper.Map<List<EmotionModel>>(queryResult.Data);
            return new Pagination<EmotionModel>(emotionModels, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }
        public async Task<ResponseModel> DeleteEmotionAsync(Guid emotionId)
        {
            var response = new ResponseModel();

            var emotion = await _unitOfWork.EmotionRepository.GetAsync(emotionId);

            if (emotion == null)
            {
                response.Status = false;
                response.Message = "Emotion not found.";
                return response;
            }

            _unitOfWork.EmotionRepository.HardDelete(emotion);
            await _unitOfWork.SaveChangeAsync();

            response.Status = true;
            response.Message = "Emotion deleted successfully.";
            return response;
        }
    }
}
