using Polaby.Repositories.Common;
using Polaby.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.RatingModel
{
    public class RatingFilterModel: PaginationParameter
    {
        public Guid? AccountId { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
