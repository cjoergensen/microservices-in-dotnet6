namespace MeterReadingService.Models;

public class MeterReading
{
    public int CustomerId { get; set; }
    public string MeterId { get; set; }
    public DateTimeOffset ReadingTime { get; set; }
    public double Value { get; set; }

    public MeterReading(int customerId, string meterId, DateTimeOffset readingTime, double value)
    {
        CustomerId = customerId;
        MeterId = meterId;
        ReadingTime = readingTime;
        Value = value;
    }
}
