namespace AcmePowerSolutions.MeterDataManagement.Api.Model;
public record MeterReading(Guid Id, int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value);