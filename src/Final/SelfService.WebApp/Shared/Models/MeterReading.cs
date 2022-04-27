namespace SelfService.WebApp.Shared.Models;

public class MeterReading
{
    public string MeterId { get; set; }
    public DateTime ReadingTime { get; set; }
    public double Value { get; set; }



    public MeterReading(string meterId, DateTime readingTime, double value)
    {
        MeterId = meterId;
        ReadingTime = readingTime;
        Value = value;
    }
}

