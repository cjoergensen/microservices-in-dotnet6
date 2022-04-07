using CustomerProfileService.Models;

namespace CustomerProfileService.Data;

public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private List<NotificationSettings> data;

    public NotificationSettingsRepository()
    {
        data = new List<NotificationSettings>
        {
            new(1, "12345678", "test1@test.com", CommunicationChannel.Phonenumber),
            new(2, "12345678", "test2@test.com", CommunicationChannel.Email)
        };
    }

    public NotificationSettings Get(int CustomerId)
    {
        return data.SingleOrDefault(x => x.CustomerId == CustomerId);
    }

}
