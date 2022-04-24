using NServiceBus;

namespace MeterReadingService.Contracts.Events.v1_0;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value) : IEvent;