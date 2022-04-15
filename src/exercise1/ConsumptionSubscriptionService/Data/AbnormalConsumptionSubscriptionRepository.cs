using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Models;

namespace ConsumptionNotificationSubscriptionService.Data;

public class AbnormalConsumptionSubscriptionRepository : IAbnormalConsumptionSubscriptionRepository
{
    private List<AbnormalConsumptionSubscription> data;

    public AbnormalConsumptionSubscriptionRepository()
    {
        data = new List<AbnormalConsumptionSubscription>
        {
            //new AbnormalConsumptionSubscription(1, CommunicationChannel.Email)
            //{
            //    CreatedOn = DateTimeOffset.Now
            //}
        };
    }

    public async Task<AbnormalConsumptionSubscription> Get(int customerId)
    {
        return data.SingleOrDefault(subscription => subscription.CustomerId == customerId);
    }

}
