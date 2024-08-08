namespace Polaby.Services.Models.ScheduleModels
{
    public class ScheduleCreateModel
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public DateOnly? Date { get; set; }
        public Guid? UserId { get; set; }
    }
}
