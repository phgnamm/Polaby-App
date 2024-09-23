namespace Polaby.Services.Models.CommentLikeModels
{
    public class CommentLikeModel
    {
        public Guid? CommentId { get; set; }
        public Guid? UserId { get; set; }
        public string? SubscriptionId { get; set; } //for OneSignal- push notification
    }
}
