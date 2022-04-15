using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.Clients;
using SelfService.WebApp.Client.Models;

namespace SelfService.WebApp.Client.Pages;

public partial class Notifications
{
    [Inject]
    public NotificationSubscriptionServiceClient? Client { get; set; }

    public bool AbnormalConsumption { get; set; }
 

    private List<NotificationSubscription>? Subscriptions { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Subscriptions = await Client!.GetSubscriptions(1);
        if (Subscriptions == null)
            AbnormalConsumption = false;
        else
            AbnormalConsumption =  Subscriptions.SingleOrDefault(subscription => subscription.Name == "abnormalconsumption").Active;

    }
}