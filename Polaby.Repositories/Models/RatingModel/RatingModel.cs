using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Models.RatingModel
{
    public class RatingModel
    {
        public int Star { get; set; }
        public string? Comment { get; set; }
        public Guid? ExpertId { get; set; }
        public Guid? UserId { get; set; }

    }
}
