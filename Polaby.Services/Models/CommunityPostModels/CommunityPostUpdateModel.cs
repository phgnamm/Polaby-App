using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommunityPostModels
{
    public class CommunityPostUpdateModel
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? Attachments { get; set; }
        public PostVisibility? Visibility { get; set; }
    }
}
