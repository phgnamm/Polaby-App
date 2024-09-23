using AutoMapper;
using Azure;
using Polaby.API.Helper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.RatingModel;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.RatingModel;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper,
            OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;
        }

        public async Task<ResponseDataModel<Rating>> CreateRatingAsync(CreateRatingModel model)
        {
            //Thấp nhất 1 sao, tối đa 5 sao
            var response = new ResponseDataModel<Rating>();
            if (model.Star < 1 || model.Star > 5)
            {
                response.Status = false;
                response.Message = "Rating must be between 1 and 5  starts";
                return response;
            }
            // Check if the User exists
            var user = await _unitOfWork.AccountRepository.GetAccountById(model.UserId);
            if (user == null)
            {
                response.Status = false;
                response.Message = "User does not exist.";
                return response;
            }

            // Check if the Expert exists
            var expert = await _unitOfWork.AccountRepository.GetAccountById(model.ExpertId);
            if (expert == null)
            {
                response.Status = false;
                response.Message = "Expert does not exist.";
                return response;
            }
            //check User chỉ được đánh giá 1 expert 1 lần duy nhất
            var existingRating = await _unitOfWork.RatingRepository.GetByUserAndExpertIdAsync(model.UserId, model.ExpertId);
            if (existingRating != null)
            {
                response.Status = true;
                response.Message = "User has already rated this expert";
                return response;
            }
            var rating = new Rating
            {
                UserId = model.UserId,
                ExpertId = model.ExpertId,
                Star = model.Star,
                Comment = model.Comment,
            };
            await _unitOfWork.RatingRepository.AddAsync(rating);
            int check = await _unitOfWork.SaveChangeAsync();

            if(check != 0)
            {
                var notificationType = await _unitOfWork.NotificationTypeRepository.GetByName(NotificationTypeName.Rate);
                var content = user.FirstName + " " + user.LastName + " " + notificationType.Content;
                _oneSignalPushNotificationService.SendNotificationAsync("Thích", content, model.SubscriptionId);
            }

            response.Status = true;
            response.Message = "Rating create successfully";
            return response;
        }

        public async Task<ResponseModel> DeleteRatingAsync(Guid ratingId)
        {
            var response = new ResponseModel();
            var rating = await _unitOfWork.RatingRepository.GetAsync(ratingId);
            if (rating == null)
            {
                response.Status = false;
                response.Message = "Rating not found.";
                return response;
            }

            // Xóa Rating
            _unitOfWork.RatingRepository.HardDelete(rating);
            await _unitOfWork.SaveChangeAsync();

            response.Status = true;
            response.Message = "Rating deleted successfully.";
            return response;
        }

        public async Task<Pagination<RatingModel>> GetRatingsByFilterAsync(RatingFilterModel model)
        {
            // Thêm filter để lọc theo UserId
            var queryResult = await _unitOfWork.RatingRepository.GetAllAsync(
                filter: r => (model.AccountId == null || r.UserId == model.AccountId || r.ExpertId == model.AccountId),  
                include: "User",                      
                pageIndex: model.PageIndex,
                pageSize: model.PageSize
            );

            var ratings = _mapper.Map<List<RatingModel>>(queryResult.Data);
            return new Pagination<RatingModel>(ratings, model.PageIndex, model.PageSize, queryResult.TotalCount);
        }


        public async Task<ResponseDataModel<Rating?>> UpdateRatingAsync(Guid id, CreateRatingModel model)
        {
            var response = new ResponseDataModel<Rating?>();

            // Tìm Rating theo ID
            var rating = await _unitOfWork.RatingRepository.GetAsync(id);
            if (rating == null)
            {
                response.Status = false;
                response.Message = "Rating not found.";
                response.Data = null;
                return response;
            }
            var user = await _unitOfWork.AccountRepository.GetAccountById(model.UserId);
            if (user == null)
            {
                response.Status = false;
                response.Message = "User does not exist.";
                return response;
            }

            var expert = await _unitOfWork.AccountRepository.GetAccountById(model.ExpertId);
            if (expert == null)
            {
                response.Status = false;
                response.Message = "Expert does not exist.";
                return response;
            }

            if (model.Star < 1 || model.Star > 5)
            {
                response.Status = false;
                response.Message = "Rating must be between 1 and 5 stars.";
                return response;
            }

            rating.Star = model.Star;
            rating.Comment = model.Comment;

            _unitOfWork.RatingRepository.Update(rating);
            await _unitOfWork.SaveChangeAsync();

            response.Status = true;
            response.Message = "Rating updated successfully.";
            response.Data = rating;

            return response;
        }

        public async Task<ResponseDataModel<RatingModel>> GetById(Guid id)
        {
            var rating = await _unitOfWork.RatingRepository.GetAsync(id);

            if (rating == null)
            {
                return new ResponseDataModel<RatingModel>()
                {
                    Status = false,
                    Message = "Rating not found"
                };
            }

            var ratingModel = _mapper.Map<RatingModel>(rating);

            return new ResponseDataModel<RatingModel>()
            {
                Status = true,
                Message = "Get rating successfully",
                Data = ratingModel
            };
        }
    }
}
