using ConsumptionNotificationSubscriptionService.Models;

namespace ConsumptionNotificationSubscriptionService.Data
{
    public interface IAbnormalConsumptionSubscriptionRepository
    {
        Task<AbnormalConsumptionSubscription> Get(int customerId);
    }
}