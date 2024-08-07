namespace Polaby.Services.Models.CommentModels
{
    public class CommentUpdateModel
    {
        public string? Content { get; set; }
        public int? LikesCount { get; set; }
        public string? Attachments { get; set; }
    }
}
