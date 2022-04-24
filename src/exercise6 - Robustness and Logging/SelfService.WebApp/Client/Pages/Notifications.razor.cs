using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.Api;

namespace SelfService.WebApp.Client.Pages;

public partial class Notifications
{
    [Inject]
    public NotificationService? NotificationService { get; set; }

    private Dictionary<string, bool>? Subscriptions { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Subscriptions = await NotificationService!.GetActiveNotifications(1);
    }

    public async Task UpdateAbnormalConsumption(bool state)
    {
        await NotificationService!.UpdateAbornormalConsumption(1, state);
    }

}