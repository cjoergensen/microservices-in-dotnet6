namespace AcmePowerSolutions.MeterDataManagement.Api.Queries;

public interface IConsumptionQueries
{
    Task<ConsumptionViewModel> GetConsumptionInPeriode(int customerId, DateTimeOffset from, DateTimeOffset to);
}
