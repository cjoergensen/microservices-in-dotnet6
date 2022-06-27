using LiteDB;
using MeterReadingService.Models;
using Microsoft.Extensions.Logging;

namespace MeterReadingService.Data;

public class MeterReadingRepository : IMeterReadingRepository
{
    private const string DatabaseFile = "Consumption.db";
    private const string CollectionName = "meterreadings";
    private readonly ILogger<MeterReadingRepository> logger;

    public MeterReadingRepository(ILogger<MeterReadingRepository> logger)
    {
        this.logger = logger;
    }

    public void Add(MeterReading meterReading)
    {
        try
        {
            logger.LogDebug("Adding new '{typeName}': {meterReading}", nameof(MeterReading), meterReading);

            using var db = new LiteDatabase($"Filename={DatabaseFile};connection=shared");
            var subscriptions = db.GetCollection<MeterReading>(CollectionName);
            subscriptions.Insert(meterReading);
            logger.LogInformation("New '{typeName}' added: {meterReading}", nameof(MeterReading), meterReading);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to add new '{typeName}'.", nameof(MeterReading));
            throw;
        }
    }

    public IEnumerable<MeterReading> GetReadings(int customerId, DateTimeOffset from, DateTimeOffset to)
    {
        using var db = new LiteDatabase(DatabaseFile);
        var readings = db.GetCollection<MeterReading>(CollectionName);
        readings.EnsureIndex(reading => reading.CustomerId);
        readings.EnsureIndex(reading => reading.ReadingTime);

        return readings.Find(reading =>
            reading.CustomerId == customerId && reading.ReadingTime >= from && reading.ReadingTime <= to).ToList();
    }
}