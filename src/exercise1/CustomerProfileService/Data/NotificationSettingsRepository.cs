using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private List<NotificationSettings> data;

    public NotificationSettingsRepository()
    {
        data = new List<NotificationSettings>
        {
            new(1, CommunicationChannel.Phonenumber),
            new(2, CommunicationChannel.Email)
        };
    }

    public NotificationSettings? Get(int CustomerId)
    {
        return data.SingleOrDefault(x => x.CustomerId == CustomerId);
    }

}
