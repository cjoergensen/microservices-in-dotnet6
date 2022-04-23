using ConsumptionNotificationSubscriptionService.Contracts.v1_0;
using ConsumptionSubscriptionService.Contracts.Commands.v1_0;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.ApiGateway.ApiClients;

public class ConsumptionNotificationSubscriptionServiceClient : IConsumptionNotificationSubscriptionServiceClient
{
    private readonly HttpClient httpClient;

    public ConsumptionNotificationSubscriptionServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> GetAbnormalConsumptionSubscriptionStatus(int customerId)
    {
        var httpResponse = await httpClient.GetAsync($"AbnormalConsumption/{customerId}");
        return httpResponse.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task CreateAbnormalConsumptionSubscription(int customerId, CommunicationChannel communicationChannel )
    {
        var command = new SubscribeToAbnormalConsumptionNotifications(customerId, communicationChannel);

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PutAsync($"abnormalconsumption/", content);
        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task DeleteAbnormalConsumptionSubscription(int customerId)
    {
        var httpResponse = await httpClient.DeleteAsync($"abnormalconsumption/{customerId}");
        httpResponse.EnsureSuccessStatusCode();
    }
}
