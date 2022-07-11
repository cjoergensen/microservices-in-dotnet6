namespace AcmePowerSolutions.MeterDataManagement.Api.Queries;

public record ConsumptionViewModel(int CustomerId, DateTimeOffset From, DateTimeOffset To, double TotalConsumption, IEnumerable<MeterReadingViewModel> Readings);

public record MeterReadingViewModel (DateTimeOffset ReadingTime, double Value);
