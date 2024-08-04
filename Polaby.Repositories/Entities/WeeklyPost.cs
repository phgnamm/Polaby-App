namespace Polaby.Repositories.Entities
{
    public class WeeklyPost : BaseEntity
    {
        public int? Week { get; set; } //Số tuần
        public string? AboutBaby { get; set; }
        public string? AboutMother { get; set; }
        public string? Advice { get; set; }
        public double? Size { get; set; } //Kích thước thai
        public double? Weight { get; set; } //Cân nặng thai
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
    }
}