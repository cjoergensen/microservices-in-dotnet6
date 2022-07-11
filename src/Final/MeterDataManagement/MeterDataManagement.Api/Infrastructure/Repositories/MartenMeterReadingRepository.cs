using AcmePowerSolutions.MeterDataManagement.Api.Model;
using Marten;

namespace AcmePowerSolutions.MeterDataManagement.Api.Infrastructure.Repositories;

public class MartenMeterReadingRepository : IMeterReadingRepository
{
    private readonly ILogger<MartenMeterReadingRepository> logger;
    private readonly IDocumentStore documentStore;

    public MartenMeterReadingRepository(ILogger<MartenMeterReadingRepository> logger, IDocumentStore documentStore)
    {
        this.logger = logger;
        this.documentStore = documentStore;
    }

    public async Task AddMeterReading(MeterReading reading)
    {
        logger.LogDebug("Inserting '{dataType}'. Data was: {data}", nameof(MeterReading), reading);
        try
        {
            using var session = documentStore.LightweightSession();
            session.Store(reading);
            await session.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to Add Meter Reading. Exception was: {exceptionMessage}", ex.Message);
            throw;
        }
    }
}
