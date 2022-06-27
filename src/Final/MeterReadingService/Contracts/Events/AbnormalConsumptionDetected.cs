namespace MeterReadingService.Contracts.v1_0.Events;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value, string MessagingId, string CorrelationId, DateTimeOffset OccuredOn) : BaseEvent(MessagingId, CorrelationId, OccuredOn);