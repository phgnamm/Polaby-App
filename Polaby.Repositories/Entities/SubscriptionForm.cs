namespace Polaby.Repositories.Entities;

public class SubscriptionForm : BaseEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}