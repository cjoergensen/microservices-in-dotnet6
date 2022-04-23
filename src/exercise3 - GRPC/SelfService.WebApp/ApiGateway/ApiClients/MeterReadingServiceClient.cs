using MeterReadingService.Contracts;
using MeterReadingService.Contracts.Queries;
using System.Text.Json;

namespace SelfService.WebApp.ApiGateway.ApiClients;

public class MeterReadingServiceClient : IMeterReadingServiceClient
{
    private readonly HttpClient httpClient;

    public MeterReadingServiceClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<Shared.Models.MeterReading>> GetMeterReadings(int customerId, DateTimeOffset from, DateTimeOffset to)
    {
        var httpResponse = await httpClient.GetAsync($"MeterReading/{customerId}?from={from:s}&to={to:s}");
        httpResponse.EnsureSuccessStatusCode();

        var content = await httpResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("Unable to load 'MeterReadings'");

        var response = JsonSerializer.Deserialize<GetMeterReadingsResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (response is null)
            throw new InvalidOperationException("Unable to load 'MeterReadings'");

        return response.MeterReadings.Select(reading => 
            new Shared.Models.MeterReading(reading.MeterId, reading.ReadingTime.LocalDateTime, reading.Value)).ToList();
    }

}