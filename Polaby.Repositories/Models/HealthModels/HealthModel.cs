using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Models.HealthModels
{
    public class HealthModel : BaseEntity
    {
        public HealthType Type { get; set; }
        public DateOnly Date { get; set; }
        public double Value { get; set; }
        public HealthUnit Unit { get; set; }
    }
}
