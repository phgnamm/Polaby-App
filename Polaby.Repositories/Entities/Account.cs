using Microsoft.AspNetCore.Identity;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Entities
{
	public class Account : IdentityUser<Guid>
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public Gender? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
		public string? Image { get; set; }
		public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }

		//Infomation of inital health
        public double? Height { get; set; }
        public double? InitalWeight { get; set; }
        public Diet? Diet { get; set; }
        public FrequencyOfActivity FrequencyOfActivity { get; set; }
		public FrequencyOfStress FrequencyOfStress { get; set; }

        //Infomation of Baby
        public string? BaybyName { get; set; }
        public Gender? BaybyGender { get; set; }
		public DateOnly DueDate { get; set; }
		public BMI? BMI { get; set; }

		//Infomation of Expert
		public string? ClinicAddress { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }
        public double? YearsOfExperience { get; set; }
		public string? Workplace { get; set; }
		public Level? Level { get; set; }

        // Subscription details
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsSubscriptionActive { get; set; }

        // Refresh Token
        public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiryTime { get; set; }

		// Email verification
		public string? VerificationCode { get; set; }
		public DateTime? VerificationCodeExpiryTime { get; set; }

        // Base Entity
        // Note: This class cannot inherit from 2 classes (BaseEntity, IdentityUser) at the same 
        public DateTime CreationDate { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime? ModificationDate { get; set; }
		public Guid? ModifiedBy { get; set; }
		public DateTime? DeletionDate { get; set; }
		public Guid? DeletedBy { get; set; }
		public bool IsDeleted { get; set; } = false;

        //Relationship
        public virtual ExpertRegistration? ExpertRegistration { get; set; }
        public virtual ICollection<Health> Healths { get; set; } = new List<Health>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Follow> Follows { get; set; } = new List<Follow>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Emotion> Emotions { get; set; } = new List<Emotion>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();
        public virtual ICollection<UserMenu> UserMenus { get; set; } = new List<UserMenu>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; } = new List<NotificationSetting>();
    }
}
