namespace Polaby.Services.Models.CommunityPostLikeModels
{
    public class CommunityPostLikeModel
    {
        public Guid? CommunityPostId { get; set; }
        public Guid? UserId { get; set; }
        public string? SubscriptionId { get; set; }
    }
}
