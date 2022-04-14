using Microsoft.AspNetCore.Components;
using SelfService.WebApp.Models;
using System.Text.Json;

namespace SelfService.WebApp.Client.Pages;

public partial class Profile
{
    [Inject]
    public HttpClient? ApiClient { get; set; }
    public Models.Profile? CustomerProfile { get; set; }
    public Models.NotificationSettings? NotificationSettings { get; set; }


    protected async override Task OnInitializedAsync()
    {
        if (ApiClient == null)
            throw new NullReferenceException("ApiClient cannot be null");

        var profileId = 1;
        CustomerProfile = await GetProfile(profileId);
        NotificationSettings = await GetNotificationSettings(profileId);

        await base.OnInitializedAsync();
    }


    private async Task<NotificationSettings> GetNotificationSettings(int profileId)
    {
        var httpResponse = await ApiClient!.GetAsync($"notificationsettings/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        var notificationSettings = JsonSerializer.Deserialize<Models.NotificationSettings>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if(notificationSettings is null)
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        return notificationSettings;
    }

    private async Task<Models.Profile> GetProfile(int profileId)
    {
        var httpResponse = await ApiClient!.GetAsync($"profile/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Profile'");

        var profile = JsonSerializer.Deserialize<Models.Profile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if(profile is null)
            throw new InvalidOperationException("Unable to load 'Profile'");

        return profile;

    }
}
