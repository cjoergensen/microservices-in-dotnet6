using ConsumptionNotificationSubscriptionService.Contracts;
using CustomerProfileService.Models;
using LiteDB;

namespace CustomerProfileService.Data;

public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private const string DatabaseFile = "CustomerProfileService.db";
    private const string CollectionName = "notificationsettings";


    public NotificationSettings? Get(int customerId)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var subscriptions = db.GetCollection<NotificationSettings>(CollectionName);
        return Get(customerId, subscriptions);
    }

    public void Update(int customerId, CommunicationChannel preferedCommunicationChannel)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var settings = db.GetCollection<NotificationSettings>(CollectionName);

        var notificationSettings = Get(customerId, settings);
        if (notificationSettings == null)
            return;

        notificationSettings.PreferedCommunicationChannel = preferedCommunicationChannel;
        settings.Update(notificationSettings);
    }

    private static NotificationSettings Get(int customerId, ILiteCollection<NotificationSettings> profiles)
    {
        profiles.EnsureIndex(subscription => subscription.CustomerId);
        return profiles.FindOne(subscription => subscription.CustomerId == customerId);
    }

}
