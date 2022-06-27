namespace MeterReadingService.Contracts.v1_0.Queries;

public record GetMeterReadingsResponse(int CustomerId, IEnumerable<MeterReading> MeterReadings);