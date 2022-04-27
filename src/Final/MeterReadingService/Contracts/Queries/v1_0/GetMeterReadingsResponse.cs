namespace MeterReadingService.Contracts.Queries.v1_0;

public record GetMeterReadingsResponse(int CustomerId, IEnumerable<MeterReading> MeterReadings);
