using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Ingredient : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool Animal { get; set; }
        public float? Kcal { get; set; }
        public float? KcalDefault { get; set; }
        public float? Weight { get; set; }
        public Unit? Unit { get; set; }
        public float? NumberOfDecimalPart { get; set; }     //soLuongPhanThapPhan
        public float? DisposalRate { get; set; }            //tyLeThaiBo
        public int? FoodGroupId { get; set; }               //nhomThucPhamId
        public int? IndexFoodGroup{ get; set; }             //sttNhomThucPham
        public string? FoodGroup { get; set; }              //strNhomThucPham
        public float? Protein { get; set; }                 //dinhDuong_Dam
        public float? Carbohydrates { get; set; }           //dinhDuong_BotDuong    
        public float? Fat { get; set; }                     //dinhDuong_Beo
        public float? Alco { get; set; }                    //dinhDuong_Alco
        public string? Source { get; set; }
        public string? SourceUrl { get; set; }
        public int? Index { get; set; }

        // Relationship
        public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        public virtual ICollection<Nutrient> Nutrients { get; set; } = new List<Nutrient>();
    }
}