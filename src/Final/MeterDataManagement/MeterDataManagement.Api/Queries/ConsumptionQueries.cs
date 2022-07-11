using AcmePowerSolutions.MeterDataManagement.Api.Model;
using Marten;

namespace AcmePowerSolutions.MeterDataManagement.Api.Queries;

public class ConsumptionQueries : IConsumptionQueries
{
    private readonly ILogger<ConsumptionQueries> logger;
    private readonly IQuerySession querySession;

    public ConsumptionQueries(ILogger<ConsumptionQueries> logger, IQuerySession querySession)
    {
        this.logger = logger;
        this.querySession = querySession;
    }

    public Task<ConsumptionViewModel> GetConsumptionInPeriode(int customerId, DateTimeOffset from, DateTimeOffset to)
    {
        logger.LogDebug("Querying for '{documentType}'. Data was CustomerId: '{customer}'; From: '{from}'; To: '{to}'.", nameof(MeterReading), customerId, from, to);
        try
        {
            var readings = querySession.Query<MeterReading>().Where(r => r.CustomerId == customerId && r.ReadingTime >= from && r.ReadingTime <= to);
            return Task.FromResult(new ConsumptionViewModel(customerId, from, to, readings.Sum(r => r.Value), 
                readings.Select(r => new MeterReadingViewModel(r.ReadingTime, r.Value)).OrderBy(nameof(MeterReading.ReadingTime))));
        }
        catch (Exception ex)
        {
            logger.LogError("Error when querying for '{documentType}'. Exception was: {exceptionMessage}", nameof(MeterReading), ex.Message);
            throw;
        }
    }
}