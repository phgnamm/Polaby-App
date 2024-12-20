﻿using AutoMapper;
using Polaby.Repositories.Interfaces;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentModels;
using Polaby.Services.Models.NewFolder;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.ResponseModels;

namespace Polaby.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<NotificationModel>> GetAllNotifications(NotificationFilterModel notificationFilterModel)
        {
            var notificationList = await _unitOfWork.NotificationRepository.GetAllAsync(
            filter: x =>
                x.IsDeleted == notificationFilterModel.IsDeleted &&
                (notificationFilterModel.IsRead == null || x.IsRead == notificationFilterModel.IsRead) &&
                (notificationFilterModel.ReceiverId == null || x.ReceiverId == notificationFilterModel.ReceiverId) &&
                (notificationFilterModel.SenderId == null || x.SenderId == notificationFilterModel.SenderId) &&
                (notificationFilterModel.CommunityPostId == null || x.CommunityPostId == notificationFilterModel.CommunityPostId) &&
                (notificationFilterModel.NotificationTypeId == null || x.NotificationTypeId == notificationFilterModel.NotificationTypeId),

            orderBy: x =>
            {
                return notificationFilterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate);
            },
            pageIndex: notificationFilterModel.PageIndex,
            pageSize: notificationFilterModel.PageSize,
            include: "Post,Receiver,Sender,NotificationType"

        );

            if (notificationList != null)
            {
                var notificationDetailList = notificationList.Data.Select(c => new NotificationModel
                {
                    Id = c.Id,
                    IsRead = c.IsRead,
                    ReceiverId = c.Receiver.Id,
                    ReceiverName = c.Receiver.FirstName + " " + c.Receiver.FirstName,
                    SenderId = c.Sender.Id,
                    SenderName = c.Sender.FirstName + " " + c.Sender.FirstName,
                    CommunityPostId = c.Post.Id,
                    CommunityPostTitle = c.Post.Title,
                    CommunityPostContent = c.Post.Content,
                    NotificationTypeId = c.NotificationType.Id,
                    NotificationTypeName = c.NotificationType.Name.ToString()
                }).ToList();

                return new Pagination<NotificationModel>(notificationDetailList, notificationFilterModel.PageIndex, notificationFilterModel.PageSize, notificationList.TotalCount);
            }
            return null;
        }

        public async Task<ResponseDataModel<NotificationModel>> GetById(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetById(id);

            if (notification == null)
            {
                return new ResponseDataModel<NotificationModel>()
                {
                    Status = false,
                    Message = "Notification not found"
                };
            }

            var notificationModel = _mapper.Map<NotificationModel>(notification);
            notificationModel.ReceiverName = notification.Receiver.FirstName + " " + notification.Receiver.LastName;
            notificationModel.SenderName = notification.Sender.FirstName + " " + notification.Sender.LastName;
            notificationModel.CommunityPostTitle = notification.Post.Title;
            notificationModel.CommunityPostContent = notification.Post.Content;
            notificationModel.NotificationTypeName = notification.NotificationType.Name.ToString();

            return new ResponseDataModel<NotificationModel>()
            {
                Status = true,
                Message = "Get otification successfully",
                Data = notificationModel
            };
        }
    }
}
