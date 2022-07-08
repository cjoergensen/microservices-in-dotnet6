namespace AcmePowerSolutions.MeterDataManagement.Api.Queries;

public interface IConsumptionQueries
{
    Task<ConsumptionViewModel> GetConsumptionInPeriode(int CustomerId, DateTimeOffset From, DateTimeOffset To);
}
