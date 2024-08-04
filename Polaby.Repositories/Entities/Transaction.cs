using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class Transaction : BaseEntity
    {
        public string? Code { get; set; }
        public string? Mesage { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public float Amount { get; set; }
        public TransactionStatus Status { get; set; }
       
        // Foreign key
        public Guid? UserId { get; set; }

        // Relationship
        public virtual Account? User { get; set; }
    }
}
