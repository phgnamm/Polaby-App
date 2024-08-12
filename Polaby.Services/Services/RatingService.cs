using AutoMapper;
using Azure;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Repositories.Models.RatingModel;
using Polaby.Repositories.Repositories;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.RatingModel;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Services
{
    public class RatingService: IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<Rating>> CreateRatingAsync(CreateRatingModel model)
        {
            //Thấp nhất 1 sao, tối đa 5 sao
            var response = new ResponseDataModel<Rating>();
            if(model.Star < 1 || model.Star > 5)
            {
                response.Status =false;
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
            if(existingRating != null)
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
            await _unitOfWork.SaveChangeAsync();
            
            response.Status = true;
            response.Message = "Rating create successfully";
            return response;
        }

        public async Task<ResponseModel> DeleteRatingAsync(Guid userId, Guid expertId)
        {
            var response = new ResponseModel();
            var rating = await _unitOfWork.RatingRepository.GetByUserAndExpertIdAsync(userId, expertId);
            if (rating == null)
            {
                response.Status = false;
                response.Message = "Rating not found.";
                return response;
            }

            _unitOfWork.RatingRepository.HardDelete(rating);
            await _unitOfWork.SaveChangeAsync();
            response.Status = true;
            response.Message = "Rating deleted successfully.";
            return response;
        }

        public async Task<Pagination<RatingModel>> GetRatingsByFilterAsync(Guid id, RatingFilterModel model)
        {
            var response = await _unitOfWork.RatingRepository.GetAsync(id);
            if (response == null)
            {
                throw new Exception("Account not found");
            }
            var queryResult = await _unitOfWork.RatingRepository.GetAllAsync(
                pageIndex: model.PageIndex,
                pageSize: model.PageSize
            );

            var ratings = _mapper.Map<List<RatingModel>>(queryResult.Data);
            return new Pagination<RatingModel>(ratings, queryResult.TotalCount, model.PageIndex, model.PageSize);
        }

        public async Task<ResponseDataModel<Rating?>> UpdateRatingAsync(CreateRatingModel model)
        {
            var response = new ResponseDataModel<Rating?>();
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

            //Thấp nhất 1 sao, tối đa 5 sao
            if (model.Star < 1 || model.Star > 5)
            {
                response.Status = false;
                response.Message = "Rating must be between 1 and 5 stars.";
                return response;
            }
            var rating = await _unitOfWork.RatingRepository.GetByUserAndExpertIdAsync(model.UserId, model.ExpertId);
            if (rating == null)
            {
                response.Status = false;
                response.Message = "Rating not found.";
                response.Data = null;
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
    }
}
