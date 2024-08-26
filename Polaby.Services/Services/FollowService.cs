using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.AccountModels;
using Polaby.Services.Models.FollowModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class FollowService : IFollowService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FollowService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResponseModel> Follow(FollowModel followModel)
        {
            var user = await _userManager.FindByIdAsync(followModel.UserId);
            if (user == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    if (role != null && !role.Equals("User"))
                    {
                        return new ResponseModel()
                        {
                            Message = "User not found",
                            Status = false
                        };
                    }
                }
            }

            var expert = await _userManager.FindByIdAsync(followModel.ExpertId);
            if (expert == null)
            {
                return new ResponseModel()
                {
                    Message = "Expert not found",
                    Status = false
                };
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(expert);
                foreach (var role in roles)
                {
                    if (role != null && !role.Equals("Expert"))
                    {
                        return new ResponseModel()
                        {
                            Message = "Expert not found",
                            Status = false
                        };
                    }
                }
            }

            Follow follow = new()
            {
                UserId = user.Id,
                ExpertId = expert.Id,
            };

            await _unitOfWork.FollowRepository.AddAsync(follow);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel()
            {
                Message = "Follow successfully!",
                Status = true
            };
        }

        public async Task<ResponseModel> Unfollow(FollowModel followModel)
        {
            var user = await _userManager.FindByIdAsync(followModel.UserId);
            if (user == null)
            {
                return new ResponseModel()
                {
                    Message = "User not found",
                    Status = false
                };
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    if (role != null && !role.Equals("User"))
                    {
                        return new ResponseModel()
                        {
                            Message = "User not found",
                            Status = false
                        };
                    }
                }
            }

            var expert = await _userManager.FindByIdAsync(followModel.ExpertId);
            if (expert == null)
            {
                return new ResponseModel()
                {
                    Message = "Expert not found",
                    Status = false
                };
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(expert);
                foreach (var role in roles)
                {
                    if (role != null && !role.Equals("Expert"))
                    {
                        return new ResponseModel()
                        {
                            Message = "Expert not found",
                            Status = false
                        };
                    }
                }
            }

            Follow follow = await _unitOfWork.FollowRepository.GetByUserAndExpert(Guid.Parse(followModel.UserId), Guid.Parse(followModel.ExpertId));

            if(follow != null)
            {
                _unitOfWork.FollowRepository.HardDelete(follow);
                await _unitOfWork.SaveChangeAsync();
            }

            return new ResponseModel()
            {
                Message = "Unfollow successfully!",
                Status = true
            };
        }
    }
}
