using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface INotificationSettingsRepository
    {
        NotificationSettings? Get(int CustomerId);
    }
}