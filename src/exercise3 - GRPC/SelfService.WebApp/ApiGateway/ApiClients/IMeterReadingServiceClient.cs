using MeterReadingService.Contracts;

namespace SelfService.WebApp.ApiGateway.ApiClients
{
    public interface IMeterReadingServiceClient
    {
        Task<IEnumerable<Shared.Models.MeterReading>> GetMeterReadings(int customerId, DateTimeOffset from, DateTimeOffset to);
    }
}