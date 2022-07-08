namespace AcmePowerSolutions.MeterDataManagement.Api.Model;
public record MeterReading(int CustomerId, string MeterId, DateTimeOffset ReadingTime, double Value);