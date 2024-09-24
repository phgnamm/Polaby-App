using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.SubscriptionFormModels;

public class SubscriptionFormCreateModel
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, ErrorMessage = "Full name must be no more than 50 characters")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required"), Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(15, ErrorMessage = "Phone number must be no more than 15 characters")]
    public required string PhoneNumber { get; set; }
}