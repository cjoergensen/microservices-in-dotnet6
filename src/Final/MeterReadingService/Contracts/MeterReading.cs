namespace MeterReadingService.Contracts.v1_0;

public record MeterReading(string MeterId, DateTimeOffset ReadingTime, double Value);