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
    public async Task<IActionResult> Get(int id, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to, [FromQuery] bool useFlicker = false)
    {
        if(useFlicker)
        {
            // simulate flickering..
            var random = new Random();
            switch (random.Next(1, 5))
            {
                case > 0 and <= 3: return StatusCode(408); // Timeout  
                default:
                    break;
            }
        }

        var readings = repository.GetReadings(id, from, to);
        var response = new Contracts.Queries.v1_0.GetMeterReadingsResponse(id,
            readings.Select(
                reading => new MeterReadingService.Contracts.MeterReading(reading.MeterId, reading.ReadingTime, reading.Value)));

        return new ObjectResult(response);
    }
}