using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Client.Api;

namespace SelfService.WebApp.Client.Pages;

public partial class Profile
{
    [Inject]
    public ProfileService? ProfileService { get; set; }
    public SelfService.WebApp.Shared.Models.Profile? CustomerProfile { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (ProfileService == null)
            throw new NullReferenceException("ApiClient cannot be null");

        var profileId = 1;
        CustomerProfile = await ProfileService.GetProfile(profileId);
        await base.OnInitializedAsync();
    }

    private async Task UpdateProfile()
    {
        await ProfileService.UpdateProfile(CustomerProfile);
    }

    //private async Task UpdateNotificationSettings()
    //{
    //    await ProfileClient.UpdateNotificationSettings(NotificationSettings);
    //}
}