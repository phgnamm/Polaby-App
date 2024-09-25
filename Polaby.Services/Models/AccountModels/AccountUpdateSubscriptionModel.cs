using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.AccountModels;

public class AccountUpdateSubscriptionModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string VerificationCode { get; set; }
}