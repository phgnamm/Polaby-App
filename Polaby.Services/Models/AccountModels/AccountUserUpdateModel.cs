using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;
using Polaby.Services.Models.AccountModels.Validation;

namespace Polaby.Services.Models.AccountModels;

public class AccountUserUpdateModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be no more than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be no more than 50 characters")]
    public required string LastName { get; set; }

    // [Required(ErrorMessage = "Gender is required")]
    // [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
    // public Gender Gender { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public DateOnly DateOfBirth { get; set; }

    // public string? Address { get; set; }
    public string? Image { get; set; }

    // [Required(ErrorMessage = "Phone number is required"), Phone(ErrorMessage = "Invalid phone format")]
    // [StringLength(15, ErrorMessage = "Phone number must be no more than 15 characters")]
    // public required string PhoneNumber { get; set; }

    // Information of initial health
    [Required(ErrorMessage = "Height is required")]
    public double Height { get; set; }

    [Required(ErrorMessage = "Initial weight is required")]
    public double InitialWeight { get; set; }

    [Required(ErrorMessage = "Diet is required")]
    public Diet Diet { get; set; }

    [Required(ErrorMessage = "Frequency of activity is required")]
    public FrequencyOfActivity FrequencyOfActivity { get; set; }

    [Required(ErrorMessage = "Frequency of stress is required")]
    public FrequencyOfStress FrequencyOfStress { get; set; }

    // Information of Baby
    public string? BabyName { get; set; }
    public Gender? BabyGender { get; set; }

    [Required(ErrorMessage = "Due date is required")]
    [DueDateValidation]
    public DateOnly DueDate { get; set; }
}