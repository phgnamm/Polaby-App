using Polaby.Repositories.Enums;

namespace Polaby.Services.Models.AccountModels;

public class AccountCreateSubscriptionModel
{
    public SubscriptionType SubscriptionType { get; set; }
    public float Price { get; set; }
}