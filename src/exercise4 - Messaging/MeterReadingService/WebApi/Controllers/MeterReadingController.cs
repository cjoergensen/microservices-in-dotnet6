using MeterReadingService.Data;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MeterReadingController : Controller
{
    private readonly IMeterReadingRepository repository;
    public MeterReadingController(IMeterReadingRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("{id?}")]
    public IActionResult Get(int id, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
    {
        var readings = repository.GetReadings(id, from, to);
        var response = new Contracts.Queries.GetMeterReadingsResponse(id,
            readings.Select(
                reading => new MeterReadingService.Contracts.MeterReading(reading.MeterId, reading.ReadingTime, reading.Value)));

        return new ObjectResult(response);
    }
}
