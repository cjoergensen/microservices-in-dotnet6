namespace MeterReadingService.Contracts.Queries;

public record GetMeterReadingsResponse(int CustomerId, IEnumerable<MeterReadingService.Contracts.MeterReading> MeterReadings);
