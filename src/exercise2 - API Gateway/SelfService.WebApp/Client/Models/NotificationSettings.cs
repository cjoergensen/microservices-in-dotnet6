using ConsumptionNotificationSubscriptionService.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SelfService.WebApp.Client.Models;

public class NotificationSettings
{
    public int CustomerId { get; }

    [Required]
    public CommunicationChannel PreferedCommunicationChannel { get; set; }

    public NotificationSettings(int customerId, CommunicationChannel preferedCommunicationChannel)
    {
        CustomerId = customerId;
        PreferedCommunicationChannel = preferedCommunicationChannel;
    }
}