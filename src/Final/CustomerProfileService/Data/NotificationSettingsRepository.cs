using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using CustomerProfileService.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace CustomerProfileService.Data;

public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private const string DatabaseFile = "CustomerProfileService.db";
    private const string CollectionName = "notificationsettings";
    private readonly ILogger<NotificationSettingsRepository> logger;

    public NotificationSettingsRepository(ILogger<NotificationSettingsRepository> logger)
    {
        this.logger = logger;
    }
    public NotificationSettings? Get(int customerId)
    {
        try
        {
            logger.LogDebug("Retrieving '{typeName}'. Customer Id = '{customerId}'", nameof(NotificationSettings), customerId);

            using var db = new LiteDatabase(DatabaseFile);
            var subscriptions = db.GetCollection<NotificationSettings>(CollectionName);
            return Get(customerId, subscriptions);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrive '{typeName}'.", nameof(NotificationSettings));
            throw;
        }
    }

    public void Update(int customerId, CommunicationChannel preferedCommunicationChannel)
    {
        try
        {
            logger.LogDebug("Updating '{typeName}'. CustomerId = '{customerId}', CommunicationChannel = '{preferedCommunicationChannel}'",nameof(NotificationSettings), customerId, preferedCommunicationChannel);

            using var db = new LiteDatabase(DatabaseFile);
            var settings = db.GetCollection<NotificationSettings>(CollectionName);

            var notificationSettings = Get(customerId, settings);
            if (notificationSettings == null)
            {
                logger.LogWarning("Update '{typeName}' was aborted. Customer with '{customerId}' was not found", nameof(NotificationSettings), customerId);
                return;
            }

            notificationSettings.PreferedCommunicationChannel = preferedCommunicationChannel;
            settings.Update(notificationSettings);
            logger.LogInformation("Updated '{typeName}': {notificationSettings}", nameof(NotificationSettings), notificationSettings);

        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to update '{typeName}'.", nameof(NotificationSettings));
            throw;
        }
    }

    private static NotificationSettings Get(int customerId, ILiteCollection<NotificationSettings> profiles)
    {
        profiles.EnsureIndex(subscription => subscription.CustomerId);
        return profiles.FindOne(subscription => subscription.CustomerId == customerId);
    }

}
