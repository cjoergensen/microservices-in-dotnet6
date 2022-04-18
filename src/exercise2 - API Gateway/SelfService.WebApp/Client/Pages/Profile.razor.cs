using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.ApiClients;

namespace SelfService.WebApp.Client.Pages;

public partial class Profile
{
    [Inject]
    public CustomerProfileServiceClient? ProfileClient { get; set; }
    public Models.Profile? CustomerProfile { get; set; }
    public Models.NotificationSettings? NotificationSettings { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (ProfileClient == null)
            throw new NullReferenceException("ApiClient cannot be null");

        var profileId = 1;
        CustomerProfile = await ProfileClient.GetProfile(profileId);
        NotificationSettings = await ProfileClient.GetNotificationSettings(profileId);

        await base.OnInitializedAsync();
    }

    private async Task UpdateProfile()
    {
        await ProfileClient.UpdateProfile(CustomerProfile);
    }

    private async Task UpdateNotificationSettings()
    {
        await ProfileClient.UpdateNotificationSettings(NotificationSettings);
    }
}