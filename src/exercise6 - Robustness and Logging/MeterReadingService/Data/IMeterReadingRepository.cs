using MeterReadingService.Models;

namespace MeterReadingService.Data;

public interface IMeterReadingRepository
{
    void Add(MeterReading meterReading);
    IEnumerable<MeterReading> GetReadings(int customerId, DateTimeOffset from, DateTimeOffset to);
}