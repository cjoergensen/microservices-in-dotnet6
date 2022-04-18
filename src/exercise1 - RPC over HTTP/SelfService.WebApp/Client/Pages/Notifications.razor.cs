using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.ApiClients;

namespace SelfService.WebApp.Client.Pages;

public partial class Notifications
{
    [Inject]
    public NotificationSubscriptionServiceClient? SubscriptionClient { get; set; }

    [Inject]
    public CustomerProfileServiceClient? ProfileClient { get; set; }

    private Dictionary<string, bool>? Subscriptions { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Subscriptions = await SubscriptionClient!.GetSubscriptions(1);
    }

    public async Task UpdateSubscription(bool newSubscription, string name)
    {
        if (newSubscription)
        {
            var notificationSettings = await ProfileClient!.GetNotificationSettings(1);
            var communicationChannel = notificationSettings.PreferedCommunicationChannel;
            await SubscriptionClient!.CreateSubscription(1, name, communicationChannel);
            return;
        }

        await SubscriptionClient!.DeleteSubscription(1, name);
    }

}