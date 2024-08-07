namespace Polaby.Repositories.Models.MenuModels
{
    public class MenuModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float? Kcal { get; set; }
        public float? Water { get; set; }
    }
}
