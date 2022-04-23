using ConsumptionNotificationSubscriptionService.Contracts;
using System.Text;
using System.Text.Json;

namespace SelfService.WebApp.Client.Api;

public class NotificationService
{
    private readonly HttpClient httpClient;

    public NotificationService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dictionary<string, bool>> GetActiveNotifications(int profileId)
    {
        var httpResponse = await httpClient.GetAsync($"notifications/{profileId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Notifications'");

        var notifications = JsonSerializer.Deserialize<Dictionary<string, bool>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (notifications is null)
            throw new InvalidOperationException("Unable to load 'Notifications'");

        return notifications;
    }

    public async Task UpdateAbornormalConsumption(int profileId, bool newState)
    {
        HttpResponseMessage httpResponse;

        if (newState)
            httpResponse = await httpClient.PutAsync($"notifications/{profileId}/abnormalconsumption", null);
        else
            httpResponse = await httpClient.DeleteAsync($"notifications/{profileId}/abnormalconsumption");

        httpResponse.EnsureSuccessStatusCode();
    }


}