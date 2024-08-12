namespace Polaby.Repositories.Entities
{
    public class Comment : BaseEntity
    {
        public string? Content { get; set; }
        public int LikesCount { get; set; }
        //public int CommentsCount { get; set; }
        public string? Attachments { get; set; }

        // Foreign key
        public Guid? AccountId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentCommentId { get; set; }

        // Relationship
        public virtual Account? Account { get; set; }
        public virtual CommunityPost? Post { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public virtual ICollection<Comment> CommentReplies { get; set; } = new List<Comment>();

    }
}