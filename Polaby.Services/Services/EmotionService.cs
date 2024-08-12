using AutoMapper;
using Azure;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.EmotionModels;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.EmotionModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var existingEmotion = await _unitOfWork.EmotionRepository.GetEmotionByDateAsync(model.UserId, model.Date);
            if (existingEmotion != null)
            {
                return new ResponseDataModel<EmotionModel>
                {
                    Status = false,
                    Message = "Emotion already exists for this date.",
                    Data = null
                };
            }

            var emotion = new Emotion
            {
                UserId = model.UserId,
                Type = model.Type,
                Date = model.Date,
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

        public async Task<ResponseDataModel<EmotionModel>> DeleteEmotionAsync(EmotionRequestModel model)
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
            var emotion = await _unitOfWork.EmotionRepository.GetEmotionByDateAsync(model.UserId, model.Date);
            if (emotion == null)
            {
                return new ResponseDataModel<EmotionModel>
                {
                    Status = false,
                    Message = "Emotion not found for this date.",
                    Data = null
                };
            }

            if (emotion.Type != model.Type)
            {
                return new ResponseDataModel<EmotionModel>
                {
                    Status = false,
                    Message = "Emotion type does not match.",
                    Data = null
                };
            }

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
