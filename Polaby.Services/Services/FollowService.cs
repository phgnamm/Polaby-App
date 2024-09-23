using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Polaby.API.Helper;
using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.FollowModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Notification;

namespace Polaby.Services.Services
{
    public class FollowService : IFollowService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public FollowService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager,
            OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;
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
            int check = await _unitOfWork.SaveChangeAsync();

            if (check != 0)
            {
                var notificationType = await _unitOfWork.NotificationTypeRepository.GetByName(NotificationTypeName.Follow);
                var content = user.FirstName + " " + user.LastName + " " + notificationType.Content;
                _oneSignalPushNotificationService.SendNotificationAsync("Thích", content, followModel.SubscriptionId);
            }

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
