using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Emotion: BaseEntity
    {
        public EmotionType? Type { get; set; }
        public DateTime Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}
