using System.Text.Json;

namespace SelfService.WebApp.Client.Api;

public class ConsumptionService
{
    private readonly HttpClient httpClient;

    public ConsumptionService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<WebApp.Shared.Models.MeterReading>> GetConsumption(int customerId)
    {
        var httpResponse = await httpClient.GetAsync($"consumption/{customerId}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'Consumption'");

        var consumption = JsonSerializer.Deserialize<IEnumerable<WebApp.Shared.Models.MeterReading>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (consumption is null)
            throw new InvalidOperationException("Unable to load 'Consumption'");

        return consumption;
    }
}
