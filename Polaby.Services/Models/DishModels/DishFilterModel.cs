﻿

using Polaby.Repositories.Common;
using Polaby.Services.Common;

namespace Polaby.Services.Models.DishModels
{
    public class DishFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "";
        public bool OrderByDescending { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid? MealId { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}
