
namespace Polaby.Services.Models.MenuModels
{
    public class MenuMealCreateModel
    {
        public Guid MenuId { get; set; }
        public List<Guid> MealIds { get; set; }
    }
}
