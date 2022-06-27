namespace MeterReadingService.Models;

public record MeterReading(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value);