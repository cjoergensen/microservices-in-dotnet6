namespace AcmePowerSolutions.MeterDataManagement.Contracts.v1_0.Events;

public record AbnormalConsumptionDetected(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value, DateTimeOffset Timestamp)
    : Shared.Messaging.BaseEvent(Timestamp);