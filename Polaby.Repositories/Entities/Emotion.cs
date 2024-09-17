using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
    public class Emotion : BaseEntity
    {
        public DateOnly? Date { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
        public virtual ICollection<EmotionTypeMapping> EmotionTypes { get; set; } = new List<EmotionTypeMapping>();
        public virtual ICollection<NoteEmotion> Notes { get; set; } = new List<NoteEmotion>();


    }
}