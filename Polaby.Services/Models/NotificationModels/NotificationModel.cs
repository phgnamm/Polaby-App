namespace Polaby.Services.Models.NewFolder
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public bool? IsRead { get; set; }
        public Guid? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public Guid? SenderId { get; set; }
        public string? SenderName { get; set; }
        public Guid? CommunityPostId { get; set; }
        public string? CommunityPostTitle { get; set; }
        public string? CommunityPostContent { get; set; }
        public Guid? NotificationTypeId { get; set; }
        public string? NotificationTypeName { get; set; }
    }
}
