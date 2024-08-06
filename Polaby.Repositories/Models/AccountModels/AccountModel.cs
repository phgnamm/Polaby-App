using Polaby.Repositories.Entities;
using Polaby.Repositories.Enums;

namespace Polaby.Repositories.Models.AccountModels;

public class AccountModel : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? Role { get; set; }
    public double? Height { get; set; }
    public double? InitialWeight { get; set; }
    public Diet? Diet { get; set; }
    public FrequencyOfActivity FrequencyOfActivity { get; set; }
    public FrequencyOfStress FrequencyOfStress { get; set; }

    // Information of Baby
    public string? BabyName { get; set; }
    public Gender? BabyGender { get; set; }
    public DateOnly DueDate { get; set; }
    public BMI? BMI { get; set; }

    // Information of Expert
    public string? ClinicAddress { get; set; }
    public string? Description { get; set; }
    public string? Education { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? Workplace { get; set; }
    public Level? Level { get; set; }

    // Subscription details
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public bool IsSubscriptionActive { get; set; }
}