using Polaby.Repositories.Enums;
using Polaby.Services.Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.EmotionModels
{
    public class EmotionRequestModel
    {
        public Guid UserId { get; set; }  
        public DateOnly Date { get; set; }  
        public List<EmotionType> EmotionTypes { get; set; } = new List<EmotionType>();  // Danh sách các cảm xúc
        public List<NoteEmotionRequestModel>? Notes { get; set; } = new List<NoteEmotionRequestModel>();  // Danh sách các ghi chú
    }
}