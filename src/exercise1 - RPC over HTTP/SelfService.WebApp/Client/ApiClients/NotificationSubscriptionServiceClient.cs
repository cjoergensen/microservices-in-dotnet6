using ConsumptionNotificationSubscriptionService.Contracts;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.ApiClients;

public class NotificationSubscriptionServiceClient
{
    private readonly HttpClient httpClient;

    public NotificationSubscriptionServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dictionary<string, bool>> GetSubscriptions(int profileId)
    {
        var result = new Dictionary<string, bool>
        {
            ["abnormalconsumption"] = false
        };

        return result;
    }

    public Task SubcribeToNotification(int profileId, string subscriptionName, CommunicationChannel communicationChannel)
    {
        return Task.CompletedTask;
    }

    public Task UnsubscribeFromNotification(int profileId, string subscriptionName)
    {
        return Task.CompletedTask;
    }
}