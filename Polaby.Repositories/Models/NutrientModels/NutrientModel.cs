
namespace Polaby.Repositories.Models.NutrientModels
{
    public class NutrientModel
    {
       // public Guid Id { get; set; } = new Guid();
        public float? PostProcessingAmount { get; set; } 
        public string? Name { get; set; }               
        public int? NutritionId { get; set; }            
        public float? ConversionRate { get; set; }       
        public float? Amount { get; set; }             
        public string? UnitName { get; set; }           
        public string? UnitCode { get; set; }           
        public int? FractionalQuantity { get; set; }      
        public int? NutritionCode { get; set; }          
    }
}
