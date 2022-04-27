using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using ConsumptionNotificationSubscriptionService.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace ConsumptionNotificationSubscriptionService.Data;

public class AbnormalConsumptionSubscriptionRepository : IAbnormalConsumptionSubscriptionRepository
{
    private const string DatabaseFile = "ConsumptionNotificationSubscriptionService.db";
    private const string CollectionName = "abnormalconsumptionsubscriptions";
    private readonly ILogger<AbnormalConsumptionSubscriptionRepository> logger;

    public AbnormalConsumptionSubscriptionRepository(ILogger<AbnormalConsumptionSubscriptionRepository> logger)
    {
        this.logger = logger;
    }

    public AbnormalConsumptionSubscription? Get(int customerId)
    {
        try
        {
            logger.LogDebug("Retrieving '{typeName}'. Customer Id = '{customerId}'", nameof(AbnormalConsumptionSubscription), customerId);
            using var db = new LiteDatabase(DatabaseFile);
            var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
            return Get(customerId, subscriptions);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrive '{typeName}'.", nameof(AbnormalConsumptionSubscription));
            throw;
        }
    }

    public void Add(int customerId, CommunicationChannel communicationChannel)
    {
        try
        {
            logger.LogDebug("Adding new '{typeName}'. Customer Id = '{customerId}', Communication Channel = '{communicationChannel}'.", nameof(AbnormalConsumptionSubscription), customerId, communicationChannel);

            using var db = new LiteDatabase(DatabaseFile);
            var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
            subscriptions.Insert(new AbnormalConsumptionSubscription(customerId, communicationChannel));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to add '{typeName}'.", nameof(AbnormalConsumptionSubscription));
            throw;
        }
    }

    public void Update(int customerId, CommunicationChannel communicationChannel)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = {customerId}, CommunicationChannel = {communicationChannel}", nameof(AbnormalConsumptionSubscription), customerId, communicationChannel);

            using var db = new LiteDatabase(DatabaseFile);
            var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
            var subscription = new AbnormalConsumptionSubscription(customerId, communicationChannel);
            subscriptions.Update(subscription);

            logger.LogInformation("Updated '{typeName}': {subscription}", nameof(AbnormalConsumptionSubscription), subscription);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update '{typeName}'.", nameof(AbnormalConsumptionSubscription));
            throw;
        }
    }

    public void Delete(int customerId)
    {
        try
        {
            logger.LogDebug("Deleting '{typeName}'. CustomerId = {customerId}.", nameof(AbnormalConsumptionSubscription), customerId);

            using var db = new LiteDatabase(DatabaseFile);
            var subscriptions = db.GetCollection<AbnormalConsumptionSubscription>(CollectionName);
            var subscription = Get(customerId, subscriptions);
            if (subscription == null)
            {
                logger.LogWarning("Delete '{typeName}' was aborted: Record with Customer Id = '{customerId}' was not found", nameof(AbnormalConsumptionSubscription), customerId);
                return;
            }
                
            subscriptions.Delete(new LiteDB.BsonValue(subscription.Id));
            logger.LogInformation("Deleted '{typeName}'. Customer Id = '{customerId}'", nameof(AbnormalConsumptionSubscription), customerId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to delete {typeName}", nameof(AbnormalConsumptionSubscription));
            throw;
        }
    }

    private static AbnormalConsumptionSubscription? Get(int customerId, ILiteCollection<AbnormalConsumptionSubscription> subscriptions)
    {
        subscriptions.EnsureIndex(subscription => subscription.CustomerId);
        return subscriptions.FindOne(subscription => subscription.CustomerId == customerId);
    }
}