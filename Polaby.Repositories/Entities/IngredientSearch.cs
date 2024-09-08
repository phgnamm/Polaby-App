using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class IngredientSearch : BaseEntity
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool Animal { get; set; }
        public float? Kcal { get; set; }
        public float? DisposalRate { get; set; }            //tyLeThaiBo
        public int? FoodGroupId { get; set; }               //nhomThucPhamId
        public int? IndexFoodGroup{ get; set; }             //sttNhomThucPham
        public string? FoodGroup { get; set; }              //strNhomThucPham
        public float? Protein { get; set; }                 //dinhDuong_Dam
        public float? Carbohydrates { get; set; }           //dinhDuong_BotDuong    
        public float? Fat { get; set; }                     //dinhDuong_Beo
        public float? Water { get; set; }                  
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }

        // Relationship
        public virtual ICollection<IngredientSearchNutrient> IngredientSearchNutrients { get; set; } = new List<IngredientSearchNutrient>();
    }
}