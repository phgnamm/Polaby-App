using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommunityPostModels
{
    public class CommunityPostModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }     
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }        
        public string? ImageUrl { get; set; }
        public string? Attachments { get; set; }       
        public bool IsProfessional { get; set; }
        public PostVisibility? Visibility { get; set; }
        public Guid? UserId { get; set; }
    }
}
