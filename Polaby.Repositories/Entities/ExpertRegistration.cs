using Polaby.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Repositories.Entities
{
    public class ExpertRegistration : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string? UrlCertificate { get; set; }
        public string? UrlCCCD { get; set; }
        public ExpertRegistrationStatus Status { get; set; }

        // Foreign key
        public Guid? ExpertId { get; set; }

        // Relationship
        public virtual Account? Expert { get; set; }
    }

}
