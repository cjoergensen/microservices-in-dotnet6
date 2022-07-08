namespace AcmePowerSolutions.MeterDataManagement.Api.Queries;

public record ConsumptionViewModel(int CustomerId, string MeterId, DateTimeOffset From, DateTimeOffset To, int TotalConsumption, List<MeterReading> Readings);

public record MeterReading (DateTimeOffset ReadingTime, int Value);
