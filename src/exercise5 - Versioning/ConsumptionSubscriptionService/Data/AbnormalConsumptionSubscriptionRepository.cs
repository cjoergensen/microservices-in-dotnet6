using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
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
        return Get(customerId, subscriptions);
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
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
        var subscription = Get(customerId, subscriptions);
        if (subscription == null)
            return;

        subscriptions.Delete(new LiteDB.BsonValue(subscription.Id));
    }

    private static AbnormalConsumptionSubscription? Get(int customerId, ILiteCollection<AbnormalConsumptionSubscription> subscriptions)
    {
        subscriptions.EnsureIndex(subscription => subscription.CustomerId);
        return subscriptions.FindOne(subscription => subscription.CustomerId == customerId);
    }
}