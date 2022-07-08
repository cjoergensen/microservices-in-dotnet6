namespace AcmePowerSolutions.MeterDataManagement.Api.Model;

public interface IMeterReadingRepository
{
    Task AddMeterReading(MeterReading reading);

}