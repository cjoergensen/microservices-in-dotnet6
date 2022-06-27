namespace MeterReadingService.Contracts.v1_0.Events;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value);