
using Polaby.Repositories.Entities;

namespace Polaby.Repositories.Models.SafeFoodModels
{
    public class SafeFoodModel : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsSafe { get; set; }
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
    }
}
