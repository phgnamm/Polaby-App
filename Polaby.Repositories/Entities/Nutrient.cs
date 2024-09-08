
namespace Polaby.Repositories.Entities
{
    public class Nutrient: BaseEntity
    {
        public float? PostProcessingAmount { get; set; }  //hamLuong_SauTT
        public string? Name { get; set; }                 //tenDinhDuong
        public int? NutritionId { get; set; }             //dinhDuongId
        public float? ConversionRate { get; set; }        //tyLeQuyDoi
        public float? Amount { get; set; }                //hamLuong
        public string? UnitName { get; set; }             //tenDonVi
        public string? UnitCode { get; set; }             //maDonVi
        public int? FractionalQuantity { get; set; }      //slPhanThapPhan
        public int? NutritionCode { get; set; }           //codeDinhDuong

        //Foreign key
        public Guid? IngredientId { get; set; }
        public Guid? DishId { get; set; }
        public Guid? MenuId { get; set; }

        //Relationship
        public virtual Ingredient? Ingredient { get; set; }
        public virtual ICollection<IngredientSearchNutrient> IngredientSearchNutrients { get; set; } = new List<IngredientSearchNutrient>();
        public virtual Dish? Dish { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
