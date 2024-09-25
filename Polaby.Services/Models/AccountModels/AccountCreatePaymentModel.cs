using System.ComponentModel.DataAnnotations;
using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.AccountModels;

public class AccountCreatePaymentModel
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Monthly;
    public string ReturnUrl { get; set; } = "";
    public string CancelUrl { get; set; } = "";
}