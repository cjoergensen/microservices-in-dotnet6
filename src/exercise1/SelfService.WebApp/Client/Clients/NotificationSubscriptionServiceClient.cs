using SelfService.WebApp.Client.Models;

namespace SelfService.WebApp.Client.Clients;

public class NotificationSubscriptionServiceClient
{
    private readonly HttpClient httpClient;

    public NotificationSubscriptionServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<NotificationSubscription>> GetSubscriptions(int profileId)
    {
        var result = new List<NotificationSubscription>();

        var httpResponse = await httpClient.GetAsync($"AbnormalConsumption/{profileId}");
        if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            result.Add(new NotificationSubscription("abnormalconsumption", true));
        else
            result.Add(new NotificationSubscription("abnormalconsumption", false));

        return result;
    }
}