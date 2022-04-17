using ConsumptionNotificationSubscriptionService.Contracts;
using ConsumptionNotificationSubscriptionService.Models;
using LiteDB;

namespace ConsumptionNotificationSubscriptionService.Data;

public class AbnormalConsumptionSubscriptionRepository : IAbnormalConsumptionSubscriptionRepository
{
    private const string DatabaseFile = "ConsumptionNotificationSubscriptionService.db";
    private const string CollectionName = "abnormalconsumptionsubscriptions";

    public AbnormalConsumptionSubscription? Get(int customerId)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
        subscriptions.EnsureIndex(subscription => subscription.CustomerId);

        return subscriptions.FindOne(subscription => subscription.CustomerId == customerId);
    }

    public void Add(int customerId, CommunicationChannel communicationChannel)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
        subscriptions.Insert(new AbnormalConsumptionSubscription(customerId, communicationChannel));
    }

    public void Update(int customerId, CommunicationChannel communicationChannel)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
        subscriptions.Update(new AbnormalConsumptionSubscription(customerId, communicationChannel));
    }

    public void Delete(int customerId)
    {
        var subscription = Get(customerId);
        if (subscription == null)
            return;
        
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
        subscriptions.Delete(new LiteDB.BsonValue(subscription.Id));
    }
}