using ConsumptionNotificationSubscriptionService.Contracts;
using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private Dictionary<int, CommunicationChannel> notificationSettings;

    public NotificationSettingsRepository()
    {

        notificationSettings = new Dictionary<int, CommunicationChannel>
        {
            [1] = CommunicationChannel.Phone,
            [2] = CommunicationChannel.Email
        };
    }

    public NotificationSettings? Get(int customerId)
    {
        if (notificationSettings.ContainsKey(customerId))
            return new NotificationSettings(customerId, notificationSettings[customerId]);

        return null;
    }

    public void Update(int customerId, CommunicationChannel preferedCommunicationChannel)
    {
        if (notificationSettings.ContainsKey(customerId))
            notificationSettings[customerId] = preferedCommunicationChannel;
    }
}
