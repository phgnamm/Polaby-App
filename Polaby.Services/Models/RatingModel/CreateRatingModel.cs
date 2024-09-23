using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.RatingModel
{
    public class CreateRatingModel
    {
        public Guid UserId { get; set; }
        public Guid ExpertId { get; set; }
        public int Star { get; set; }
        public string? Comment { get; set; }
        public string? SubscriptionId { get; set; }
    }
}
