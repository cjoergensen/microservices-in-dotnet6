using ConsumptionNotificationSubscriptionService.Contracts.v1_0;

namespace CustomerProfileService.Models;

public class NotificationSettings
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public CommunicationChannel PreferedCommunicationChannel { get; set; }
}