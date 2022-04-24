namespace MeterReadingService.Contracts;

public record MeterReading(string MeterId, DateTimeOffset ReadingTime, double Value);
