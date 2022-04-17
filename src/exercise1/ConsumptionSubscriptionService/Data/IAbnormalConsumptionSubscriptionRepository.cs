using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Models;

namespace ConsumptionNotificationSubscriptionService.Data
{
    public interface IAbnormalConsumptionSubscriptionRepository
    {
        void Add(int customerId, CommunicationChannel communicationChannel);
        void Delete(int customerId);
        AbnormalConsumptionSubscription? Get(int customerId);
        void Update(int customerId, CommunicationChannel communicationChannel);
    }
}