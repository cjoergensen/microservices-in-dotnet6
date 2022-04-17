using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Models;

namespace ConsumptionNotificationSubscriptionService.Data;

public class AbnormalConsumptionSubscriptionRepository : IAbnormalConsumptionSubscriptionRepository
{
    private Dictionary<int, AbnormalConsumptionSubscription> data;

    public AbnormalConsumptionSubscriptionRepository()
    {
        data = new();
    }

    public AbnormalConsumptionSubscription? Get(int customerId)
    {
        if(data.ContainsKey(customerId))
            return data[customerId];

        return null;
    }

    public void Add(int customerId, CommunicationChannel communicationChannel)
    {
        var subscription = Get(customerId);
        if (subscription == null)
            data.Add(customerId, new AbnormalConsumptionSubscription(customerId, communicationChannel));
        else
            data[customerId] = new AbnormalConsumptionSubscription(customerId, communicationChannel);
    }

    public void Update(int customerId, CommunicationChannel communicationChannel)
    {
        var subscription = Get(customerId);
        if (subscription == null)
            return;

        data[customerId] = new AbnormalConsumptionSubscription(customerId, communicationChannel);
    }

    public void Delete(int customerId)
    {
        var subscription = Get(customerId);
        if (subscription != null)
            data.Remove(customerId);
    }

}
