using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.Clients;

namespace SelfService.WebApp.Client.Pages;

public partial class Profile
{
    [Inject]
    public CustomerProfileServiceClient? Client { get; set; }
    public SelfService.WebApp.Models.Profile? CustomerProfile { get; set; }
    public SelfService.WebApp.Models.NotificationSettings? NotificationSettings { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (Client == null)
            throw new NullReferenceException("ApiClient cannot be null");

        var profileId = 1;
        CustomerProfile = await Client.GetProfile(profileId);
        NotificationSettings = await Client.GetNotificationSettings(profileId);

        await base.OnInitializedAsync();
    }
}