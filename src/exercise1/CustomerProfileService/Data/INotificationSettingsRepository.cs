using ConsumptionNotificationSubscriptionService.Contracts;
using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface INotificationSettingsRepository
    {
        NotificationSettings? Get(int customerId);
        void Update(int customerID, CommunicationChannel preferedCommunicationChannel);
    }
}