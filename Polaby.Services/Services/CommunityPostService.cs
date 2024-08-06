using AutoMapper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Services
{
    public class CommunityPostService : ICommunityPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommunityPostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<CommunityPostModel>> Create(CommunityPostCreateModel communityPostCreateModel)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)communityPostCreateModel.UserId);
            if (account == null)
            {
                return new ResponseDataModel<CommunityPostModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            CommunityPost communityPost = _mapper.Map<CommunityPost>(communityPostCreateModel);

            await _unitOfWork.CommuntityPostRepository.AddAsync(communityPost);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<CommunityPostModel>(communityPost);
            return new ResponseDataModel<CommunityPostModel>()
            {
                Message = "Create post successfully!",
                Status = true,
                Data = result
            };
        }
    }
}
