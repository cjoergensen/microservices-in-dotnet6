using SelfService.WebApp.Models;
using System.Text.Json;

namespace SelfService.WebApp.Client.Clients;

public class CustomerProfileServiceClient
{
    private readonly HttpClient httpClient;

    public CustomerProfileServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<NotificationSettings> GetNotificationSettings(int profileId)
    {
        var httpResponse = await httpClient.GetAsync($"notificationsettings/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        var notificationSettings = JsonSerializer.Deserialize<NotificationSettings>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (notificationSettings is null)
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        return notificationSettings;
    }

    public async Task<Profile> GetProfile(int profileId)
    {
        var httpResponse = await httpClient.GetAsync($"profile/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Profile'");

        var profile = JsonSerializer.Deserialize<Profile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (profile is null)
            throw new InvalidOperationException("Unable to load 'Profile'");

        return profile;

    }
}
