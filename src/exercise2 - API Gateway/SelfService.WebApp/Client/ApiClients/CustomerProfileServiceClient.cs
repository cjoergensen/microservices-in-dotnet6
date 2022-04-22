using CustomerProfileService.Contracts.Commands;
using SelfService.WebApp.Client.Models;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.ApiClients;

public class CustomerProfileServiceClient
{
    private readonly HttpClient httpClient;

    public CustomerProfileServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
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

    public async Task UpdateProfile(Profile profile)
    {
        var updateCommand = new UpdateProfile(profile.CustomerId, profile.Name, profile.PhoneNumber, profile.Email);
        var content = new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"profile", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task<NotificationSettings> GetNotificationSettings(int profileId)
    {
        var httpResponse = await httpClient.GetAsync($"notificationsettings/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new NotificationSettings(profileId, ConsumptionNotificationSubscriptionService.Contracts.CommunicationChannel.Email);

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        var notificationSettings = JsonSerializer.Deserialize<NotificationSettings>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (notificationSettings is null)
            throw new InvalidOperationException("Unable to load 'Notification Settings'");

        return notificationSettings;
    }

    public async Task UpdateNotificationSettings(NotificationSettings notificationSettings)
    {
        var updateCommand = new UpdateNotificationSettings(notificationSettings.CustomerId, notificationSettings.PreferedCommunicationChannel);
        var content = new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"notificationsettings", content);
        httpResponse.EnsureSuccessStatusCode();
    }


}
