using CustomerProfileService.Contracts.v1_0.Commands;
using CustomerProfileService.Contracts.v1_0.Queries;
using NServiceBus;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SelfService.WebApp.ApiGateway.ApiClients;

public class CustomerProfileServiceClient : ICustomerProfileServiceClient
{
    private readonly HttpClient httpClient;
    private readonly IMessageSession messageSession;

    public CustomerProfileServiceClient(IMessageSession messageSession, HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.messageSession = messageSession;
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
        var tasks = new List<Task>(2);

        var updateProfileCommand = new CustomerProfileService.Contracts.v1_0.Commands.UpdateProfile(profile.Id, profile.Name, profile.PhoneNumber, profile.Email);
        tasks.Add(messageSession.Send("CustomerProfileService", updateProfileCommand));

        var updateNotificationSettings = new UpdateNotificationSettings(profile.CustomerId, profile.PreferedCommunicationChannel);
        tasks.Add(messageSession.Send("CustomerProfileService", updateNotificationSettings));
        
        await Task.WhenAll(tasks);
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

        var getNotificationSettingsResponse = System.Text.Json.JsonSerializer.Deserialize<GetNotificationSettingsResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (getNotificationSettingsResponse is null)
            throw new InvalidOperationException("Unable to load 'NotificationSettings'");

        return getNotificationSettingsResponse;
    }
}