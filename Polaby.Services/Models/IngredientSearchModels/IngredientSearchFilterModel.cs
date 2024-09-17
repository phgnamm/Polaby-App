

using Polaby.Repositories.Common;
using Polaby.Repositories.Enums;
using Polaby.Services.Common;

namespace Polaby.Services.Models.IngredientSearchModels
{
    public class IngredientSearchFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "";
        public bool OrderByDescending { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public FoodGroup? FoodGroup { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = 200;
    }
}
