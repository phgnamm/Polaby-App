using Polaby.Repositories.Common;
using Polaby.Repositories.Enums;
using Polaby.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommentModels
{
    public class CommentFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public Guid? AccountId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
