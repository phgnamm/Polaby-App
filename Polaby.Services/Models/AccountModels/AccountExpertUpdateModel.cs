using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.AccountModels;

public class AccountExpertUpdateModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be no more than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be no more than 50 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public DateOnly DateOfBirth { get; set; }

    public string? Image { get; set; }

    // Information of Expert
    [Required(ErrorMessage = "Clinic address is required")]
    public string ClinicAddress { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Education is required")]
    public string Education { get; set; }

    [Required(ErrorMessage = "Years of experience is required")]
    public int YearsOfExperience { get; set; }

    [Required(ErrorMessage = "Workplace is required")]
    public string Workplace { get; set; }

    [Required(ErrorMessage = "Level is required")]
    public Level Level { get; set; }
}