namespace Polaby.Repositories.Entities
{
    public class SafeFood : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsSafe { get; set; }
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
        public string? ImageUrl { get; set; }
    }
}