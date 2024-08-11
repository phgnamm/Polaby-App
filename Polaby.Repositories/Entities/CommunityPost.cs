using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class CommunityPost : BaseEntity
    {
        public string? Title { get; set; }              
        public string? Content { get; set; }             
        // public bool IsPublished { get; set; }          
        // public string? Tags { get; set; }            
        public int LikesCount { get; set; }             
        public int CommentsCount { get; set; }          
        // public int ViewsCount { get; set; }             
        public string? ImageUrl { get; set; }           
        public string? Attachments { get; set; }       
        // public bool IsArchived { get; set; }           
        public bool IsProfessional { get; set; }        
        public PostVisibility? Visibility { get; set; }

        // Foreign key
        public Guid? AccountId { get; set; }

        // Relationship
        public virtual Account? Account { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<CommunityPostLike> CommunityPostLikes { get; set; } = new List<CommunityPostLike>();

    }
}
