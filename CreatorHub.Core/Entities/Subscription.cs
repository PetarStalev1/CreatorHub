using CreatorHub.Core.Enums;

namespace CreatorHub.Core.Entities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Free;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public DateTime CurrentPeriodEnd { get; set; }
    public string? StripeSubId { get; set; }

    public User User { get; set; } = null!;
}