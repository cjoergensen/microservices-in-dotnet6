using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using CustomerProfileService.Models;

namespace CustomerProfileService.Data
{
    public interface INotificationSettingsRepository
    {
        NotificationSettings? Get(int customerId);
        Task Update(int customerID, CommunicationChannel preferedCommunicationChannel);
    }
}