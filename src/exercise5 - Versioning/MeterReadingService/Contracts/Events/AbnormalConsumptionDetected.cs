using NServiceBus;

namespace MeterReadingService.Contracts.Events;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, Double Value) : IEvent;