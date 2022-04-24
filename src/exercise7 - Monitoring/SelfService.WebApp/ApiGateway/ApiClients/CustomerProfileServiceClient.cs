using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Contracts.v1_0.Queries;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.ApiGateway.ApiClients;

public class CustomerProfileServiceClient : ICustomerProfileServiceClient
{
    private readonly HttpClient httpClient;

    public CustomerProfileServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Shared.Models.Profile> GetProfile(int profileId)
    {
        var customerProfile = await GetCustomerProfile(profileId);
        var notificationSettings = await GetNotificationSettings(profileId);

        return new Shared.Models.Profile
        {
            CustomerId = customerProfile.CustomerId,
            Email = customerProfile.Email,
            Name = customerProfile.Name,
            PhoneNumber = customerProfile.PhoneNumber,
            PreferedCommunicationChannel = notificationSettings.PreferedCommunicationChannel
        };
    }

    public async Task UpdateProfile(Shared.Models.Profile profile)
    {
        await UpdateProfile(new UpdateProfile(profile.CustomerId, profile.Name, profile.PhoneNumber, profile.Email));
        await UpdateNotificationSettings(new UpdateNotificationSettings(profile.CustomerId, profile.PreferedCommunicationChannel));
    }

    private async Task<GetCustomerProfileResponse> GetCustomerProfile(int customerId)
    {
        var httpResponse = await httpClient.GetAsync($"profile/{customerId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Profile'");

        var getCustomerProfileResponse = JsonSerializer.Deserialize<GetCustomerProfileResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (getCustomerProfileResponse is null)
            throw new InvalidOperationException("Unable to load 'Profile'");

        return getCustomerProfileResponse;
    }

    private async Task<GetNotificationSettingsResponse> GetNotificationSettings(int customerId)
    {
        var httpResponse = await httpClient.GetAsync($"notificationsettings/{customerId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'NotificationSettings'");

        var getNotificationSettingsResponse = JsonSerializer.Deserialize<GetNotificationSettingsResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (getNotificationSettingsResponse is null)
            throw new InvalidOperationException("Unable to load 'NotificationSettings'");

        return getNotificationSettingsResponse;
    }

    private async Task UpdateProfile(CustomerProfileService.Contracts.v1_0.Commands.UpdateProfile command)
    {
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"profile", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    private async Task UpdateNotificationSettings(UpdateNotificationSettings command)
    {
        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"notificationsettings", content);
        httpResponse.EnsureSuccessStatusCode();
    }
}