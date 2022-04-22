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

    public Task<NotificationSettings> GetNotificationSettings(int customerId)
    {
        return Task.FromResult(new NotificationSettings(customerId, ConsumptionNotificationSubscriptionService.Contracts.CommunicationChannel.Phone));
    }

    public Task<Profile> GetProfile(int customerId)
    {
        return Task.FromResult(new Profile(customerId, "John Doe", "12345678", "john@test.com"));
    }

    public Task UpdateNotificationSettings(NotificationSettings notificationSettings)
    {
        return Task.CompletedTask;
    }

    public Task UpdateProfile(Profile profile)
    {
        return Task.CompletedTask;
    }
}
