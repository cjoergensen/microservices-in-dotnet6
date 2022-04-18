using ConsumptionNotificationSubscriptionService.Contracts;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.Clients;

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

        var httpResponse = await httpClient.GetAsync($"AbnormalConsumption/{profileId}");
        if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            result["abnormalconsumption"] = true;

        return result;
    }

    public async Task CreateSubscription(int profileId, string subscriptionName, CommunicationChannel communicationChannel)
    {
        var content = new StringContent(JsonSerializer.Serialize(communicationChannel), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"{subscriptionName}/{profileId}", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task DeleteSubscription(int profileId, string subscriptionName)
    {
        var httpResponse = await httpClient.DeleteAsync($"{subscriptionName}/{profileId}");
        httpResponse.EnsureSuccessStatusCode();
    }
}