using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.ExpertRegistrationModels;

public class ExpertRegistrationUpdateModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name must be no more than 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be no more than 50 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public DateOnly DateOfBirth { get; set; }

    public string? Address { get; set; }

    [Required(ErrorMessage = "Phone number is required"), Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(15, ErrorMessage = "Phone number must be no more than 15 characters")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
    public required string Email { get; set; }

    // public string? Image { get; set; }

    [Required(ErrorMessage = "Certificate is required")]
    public string CertificateUrl { get; set; }

    [Required(ErrorMessage = "Clinic license is required")]
    public string ClinicLicenseUrl { get; set; }

    [Required(ErrorMessage = "Front CCCD is required")]
    public string CCCDFrontUrl { get; set; }

    [Required(ErrorMessage = "Back CCCD is required")]
    public string CCCDBackUrl { get; set; }

    [Required(ErrorMessage = "Clinic address is required")]
    public string ClinicAddress { get; set; }

    [Required(ErrorMessage = "Description is required")]
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