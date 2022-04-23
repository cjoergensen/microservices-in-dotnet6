using LiteDB;
using MeterReadingService.Models;

namespace MeterReadingService.Data;

public class MeterReadingRepository : IMeterReadingRepository
{
    private const string DatabaseFile = "Consumption.db";
    private const string CollectionName = "meterreadings";

    public void Add(MeterReading meterReading)
    {
        using var db = new LiteDatabase($"Filename={DatabaseFile};connection=shared");
        var subscriptions = db.GetCollection<MeterReading>(CollectionName);
        subscriptions.Insert(meterReading);
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
